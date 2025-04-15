using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace lab5
{
    public partial class Form1 : Form
    {
        private List<Movie> currentMovies = new List<Movie>();

        public Form1()
        {
            InitializeComponent();
            ConfigureDataGridView();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                var genres = await TmdbApi.GetGenresAsync();
                comboGenre.DataSource = genres;
                comboGenre.DisplayMember = "name";
                comboGenre.ValueMember = "id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки жанров: " + ex.Message);
            }
        }

        private async void btnFind_Click(object sender, EventArgs e)
        {
            var selectedGenre = (TmdbGenre)comboGenre.SelectedItem;
            double minRating = (double)numericRating.Value;
            int yearFrom = (int)numericYearFrom.Value;
            int yearTo = (int)numericYearTo.Value;

            dgvMovies.DataSource = null;
            currentMovies = await MovieFinder.FindMoviesAsync(selectedGenre.id, minRating, yearFrom, yearTo);
            dgvMovies.DataSource = currentMovies;
        }

        private void dgvMovies_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvMovies_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMovies.SelectedRows.Count > 0)
            {
                var movie = dgvMovies.SelectedRows[0].DataBoundItem as Movie;
                if (movie != null)
                {
                    lblTitle.Text = movie.Title + " (" + movie.Year + ")";
                    lblRating.Text = movie.Rating;
                    txtDescription.Text = movie.Description;

                    try
                    {
                        using (var client = new HttpClient())
                        {
                            var imageBytes = client.GetByteArrayAsync(movie.PosterUrl).Result;
                            using (var ms = new MemoryStream(imageBytes))
                            {
                                picturePoster.Image = Image.FromStream(ms);
                            }
                        }
                    }
                    catch
                    {
                        picturePoster.Image = null;
                    }
                }
            }
        }

        private void btnExportJson_Click(object sender, EventArgs e)
        {
            if (currentMovies.Count == 0)
            {
                MessageBox.Show("Сначала выполните поиск.");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "JSON (*.json)|*.json";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Exporter.ExportToJson(currentMovies, sfd.FileName);
                }
            }
        }

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            if (currentMovies.Count == 0)
            {
                MessageBox.Show("Сначала выполните поиск.");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV (*.csv)|*.csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Exporter.ExportToCsv(currentMovies, sfd.FileName);
                }
            }
        }

        private void btnCopyLink_Click(object sender, EventArgs e)
        {
            if (currentMovies.Count == 0)
            {
                MessageBox.Show("Сначала выполните поиск.");
                return;
            }

            string link = Exporter.GenerateShareLink(currentMovies);
            if (!string.IsNullOrEmpty(link))
            {
                Clipboard.SetText(link);
                MessageBox.Show("Ссылка скопирована в буфер обмена.");
            }
            else
            {
                MessageBox.Show("Не удалось сгенерировать ссылку.");
            }
        }
        private void ConfigureDataGridView()
        {
            dgvMovies.AutoGenerateColumns = false;
            dgvMovies.Columns.Clear();

            dgvMovies.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Название",
                DataPropertyName = "Title",
            });

            dgvMovies.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Год",
                DataPropertyName = "Year",
            });

            dgvMovies.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Рейтинг",
                DataPropertyName = "Rating",
            });

            dgvMovies.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Жанры",
                DataPropertyName = "Genre",
            });
        }
    }
}
