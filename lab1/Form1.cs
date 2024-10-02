using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1
{
    public partial class Form1 : Form
    {
        private Panel variablesPanel;
        private Panel constraintsPanel;
        private const int Padding = 10; // Отступы между элементами
        private const int ElementHeight = 25;
        private const int MinWidth = 50;


        public Form1()
        {
            InitializeComponent();
            // Установка размеров и свойств формы
            this.Text = "Симплекс-Калькулятор";
            //this.Size = new Size(800, 600);

            // Панель для переменных
            variablesPanel = new Panel
            {
                Location = new Point(10, 100),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            // Панель для ограничений
            constraintsPanel = new Panel
            {
                Location = new Point(10, 250),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            // Добавляем панели на форму
            this.Controls.Add(variablesPanel);
            this.Controls.Add(constraintsPanel);

            // Добавляем события для автоматического обновления при изменении значений
            textBox1.TextChanged += TextBoxes_TextChanged;
            textBox2.TextChanged += TextBoxes_TextChanged;
        }

        private void TextBoxes_TextChanged(object sender, EventArgs e)
        {
            // Очищаем панели перед созданием новых элементов
            variablesPanel.Controls.Clear();
            constraintsPanel.Controls.Clear();

            // Используем существующие textbox1 и textbox2
            if (int.TryParse(textBox1.Text, out int numVariables) && int.TryParse(textBox2.Text, out int numConstraints))
            {
                CreateVariableInputs(numVariables);
                CreateConstraintInputs(numConstraints, numVariables);
            }
            else
            {
                //MessageBox.Show("Введите корректные числа для переменных и ограничений.");
                return;
            }
        }

        private void CreateVariableInputs(int numVariables)
        {
            Label objectiveLabel = new Label
            {
                Text = "Целевая функция:",
                AutoSize = true,
                Location = new Point(10, 10)
            };
            variablesPanel.Controls.Add(objectiveLabel);

            // Рассчитываем ширину полей в зависимости от количества переменных и доступного места
            int panelWidth = this.ClientSize.Width - 40;
            int totalWidth = panelWidth - (Padding * 2 * numVariables);
            int fieldWidth = Math.Max(MinWidth, totalWidth / numVariables);

            for (int i = 0; i < numVariables; i++)
            {
                TextBox variableTextBox = new TextBox
                {
                    Width = fieldWidth,
                    Location = new Point(i * (fieldWidth + Padding * 2), 40)
                };

                Label variableLabel = new Label
                {
                    Text = $"x{i + 1}",
                    AutoSize = true,
                    Location = new Point(variableTextBox.Location.X + fieldWidth, 44) // Метка справа от TextBox
                };

                variablesPanel.Controls.Add(variableTextBox);
                variablesPanel.Controls.Add(variableLabel);
            }

            // Радиокнопки для выбора типа задачи (максимизация/минимизация)
            RadioButton maxRadioButton = new RadioButton
            {
                Text = "Максимизация",
                Location = new Point(10, 70),
                AutoSize = true
            };
            RadioButton minRadioButton = new RadioButton
            {
                Text = "Минимизация",
                Location = new Point(120, 70),
                AutoSize = true
            };
            minRadioButton.Checked = true; // По умолчанию минимизация

            variablesPanel.Controls.Add(maxRadioButton);
            variablesPanel.Controls.Add(minRadioButton);
        }

        private void CreateConstraintInputs(int numConstraints, int numVariables)
        {
            Label constraintsLabel = new Label
            {
                Text = "Ограничения:",
                AutoSize = true,
                Location = new Point(10, 10)
            };
            constraintsPanel.Controls.Add(constraintsLabel);

            int panelWidth = this.ClientSize.Width - 40;
            int totalWidth = panelWidth - (Padding * 2 * (numVariables + 2));
            int fieldWidth = Math.Max(MinWidth, totalWidth / (numVariables + 2));

            // Создаем строку для каждого ограничения
            for (int i = 0; i < numConstraints; i++)
            {
                int yPosition = 40 + i * (ElementHeight + Padding); // Вычисляем Y-позицию для каждого ограничения

                // Создаем набор текстовых полей и меток для каждого ограничения
                for (int j = 0; j < numVariables; j++)
                {
                    TextBox variableTextBox = new TextBox
                    {
                        Width = fieldWidth,
                        Location = new Point(j * (fieldWidth + Padding * 2), yPosition)
                    };

                    Label variableLabel = new Label
                    {
                        Text = $"x{j + 1}",
                        AutoSize = true,
                        Location = new Point(variableTextBox.Location.X + fieldWidth, yPosition + 4) // Метка справа от TextBox
                    };

                    constraintsPanel.Controls.Add(variableTextBox);
                    constraintsPanel.Controls.Add(variableLabel);
                }

                // Комбобокс для типа ограничения
                ComboBox constraintType = new ComboBox
                {
                    Width = fieldWidth,
                    Location = new Point(numVariables * (fieldWidth + Padding * 2), yPosition),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                constraintType.Items.AddRange(new[] { "<=", ">=", "=" });
                constraintType.SelectedIndex = 0;
                constraintsPanel.Controls.Add(constraintType);

                // Поле для ввода правой части уравнения
                TextBox rightSide = new TextBox
                {
                    Width = fieldWidth,
                    Location = new Point((numVariables + 1) * (fieldWidth + Padding * 2), yPosition)
                };
                constraintsPanel.Controls.Add(rightSide);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
