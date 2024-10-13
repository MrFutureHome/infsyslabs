using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1WithoutEnhancements
{
    public partial class Form1 : Form
    {
        private Panel variablesPanel;
        private Panel constraintsPanel;
        private TextBox[,] textBoxArray;
        private TextBox[] textBoxObjectiveArray;
        private const int Padding = 10; // Отступы между элементами
        
        public Form1()
        {
            InitializeComponent();
            // Установка размеров и свойств формы
            this.Text = "Симплекс-Калькулятор";
            //this.Size = new Size(800, 600);

            int newPanelYPosition = textBox2.Location.Y + textBox2.Height + Padding;

            // Панель для переменных
            variablesPanel = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Top
            };

            // Панель для ограничений
            constraintsPanel = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Top
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

            int newPanelYPosition = textBox2.Location.Y + textBox2.Height + Padding;
            variablesPanel.Location = new Point(10, newPanelYPosition + 200);
            constraintsPanel.Location = new Point(10, variablesPanel.Location.Y + variablesPanel.Height + Padding + 200);
        }

        private void CreateVariableInputs(int numVariables)
        {
            // Создаем массив TextBox для коэффициентов целевой функции
            textBoxObjectiveArray = new TextBox[numVariables];

            Label objectiveLabel = new Label();
            objectiveLabel.Text = "Коэффициенты целевой функции:";
            objectiveLabel.AutoSize = true;
            objectiveLabel.Location = new Point(0, 20); // Устанавливаем локальную позицию
            variablesPanel.Controls.Add(objectiveLabel); // Добавляем на панель

            int totalWidth = numVariables * 100; // Общая ширина всех TextBox'ов
            int startX = (variablesPanel.ClientSize.Width - totalWidth) / 2; // Центрирование всех TextBox'ов относительно панели

            for (int i = 0; i < numVariables; i++)
            {
                textBoxObjectiveArray[i] = new TextBox();
                textBoxObjectiveArray[i].Location = new Point(startX + i * 100, 50);
                textBoxObjectiveArray[i].Size = new Size(80, 20);
                textBoxObjectiveArray[i].Anchor = AnchorStyles.Top | AnchorStyles.Left;
                variablesPanel.Controls.Add(textBoxObjectiveArray[i]); // Добавляем на панель

                Label varLabel = new Label();
                varLabel.Text = $"x{i + 1}";
                varLabel.AutoSize = true;
                varLabel.Location = new Point(startX + i * 100, 30); // Позиционирование метки над TextBox
                variablesPanel.Controls.Add(varLabel); // Добавляем на панель
            }
        }

        private void CreateConstraintInputs(int numConstraints, int numVariables)
        {
            // Очистка предыдущих полей ввода для ограничений
            if (textBoxArray != null)
            {
                foreach (var textBox in textBoxArray)
                {
                    constraintsPanel.Controls.Remove(textBox); // Удаляем из панели ограничений
                }
            }

            // Создаем массив TextBox для ограничений
            textBoxArray = new TextBox[numConstraints, numVariables + 1]; // +1 для свободных членов

            Label constraintsLabel = new Label();
            constraintsLabel.Text = "Ограничения:";
            constraintsLabel.AutoSize = true;
            constraintsLabel.Location = new Point(0, 100); // Устанавливаем локальную позицию
            constraintsPanel.Controls.Add(constraintsLabel); // Добавляем на панель

            int totalWidth = (numVariables + 1) * 100 + 20; // Общая ширина всех TextBox'ов и знаков <=
            int startX = (constraintsPanel.ClientSize.Width - totalWidth) / 2; // Центрирование всех TextBox'ов относительно панели

            for (int i = 0; i < numConstraints; i++)
            {
                for (int j = 0; j < numVariables; j++)
                {
                    textBoxArray[i, j] = new TextBox();
                    textBoxArray[i, j].Location = new Point(startX + j * 100, 130 + i * 30); // Позиционирование TextBox
                    textBoxArray[i, j].Size = new Size(80, 20);
                    textBoxArray[i, j].Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    constraintsPanel.Controls.Add(textBoxArray[i, j]); // Добавляем на панель

                    if (i == 0)
                    {
                        Label varLabel = new Label();
                        varLabel.Text = $"x{j + 1}";
                        varLabel.AutoSize = true;
                        varLabel.Location = new Point(startX + j * 100, 110); // Позиционирование метки
                        constraintsPanel.Controls.Add(varLabel); // Добавляем на панель
                    }
                }

                // Добавляем метку "<="
                Label inequalityLabel = new Label();
                inequalityLabel.Text = "≤";
                inequalityLabel.AutoSize = true;
                inequalityLabel.Location = new Point(startX + numVariables * 100, 130 + i * 30);
                constraintsPanel.Controls.Add(inequalityLabel); // Добавляем на панель

                // Свободный член (справа от знака "<=")
                textBoxArray[i, numVariables] = new TextBox();
                textBoxArray[i, numVariables].Location = new Point(startX + (numVariables + 1) * 100 - 60, 130 + i * 30);
                textBoxArray[i, numVariables].Size = new Size(80, 20);
                textBoxArray[i, numVariables].Anchor = AnchorStyles.Top | AnchorStyles.Left;
                constraintsPanel.Controls.Add(textBoxArray[i, numVariables]); // Добавляем на панель
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int numVariables = textBoxObjectiveArray.Length; // Количество переменных
                int numConstraints = textBoxArray.GetLength(0); // Количество ограничений

                double[,] table = new double[numConstraints + 1, numVariables + 1];

                // Заполнение коэффициентов ограничений
                for (int i = 0; i < numConstraints; i++)
                {
                    double[] constraintCoefficients = new double[numVariables];
                    for (int j = 0; j < numVariables; j++)
                    {
                        constraintCoefficients[j] = Convert.ToDouble(textBoxArray[i, j].Text);
                    }
                    table[i, 0] = Convert.ToDouble(textBoxArray[i, numVariables].Text); // Свободный член
                    for (int j = 0; j < numVariables; j++)
                    {
                        table[i, j + 1] = constraintCoefficients[j];
                    }
                }

                // Заполнение целевой функции
                for (int j = 0; j < numVariables; j++)
                {
                    table[numConstraints, j + 1] = -Convert.ToDouble(textBoxObjectiveArray[j].Text);
                }

                double[] result = new double[numVariables];
                Simplex simplex = new Simplex(table);
                double[,] tableResult = simplex.Calculate(result);

                // Вывод результата в MessageBox
                string message = "Решение:\n";
                for (int i = 0; i < numVariables; i++)
                {
                    message += $"x{i + 1} = {Math.Round(result[i], 3)}\n";
                }
                message += "\nЦелевая функция: " + Math.Round(tableResult[numConstraints, 0], 3);

                MessageBox.Show(message, "Результат");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка");
            }
        }

        public class Simplex
        {
            double[,] table;
            int m, n;
            List<int> basis;

            public Simplex(double[,] source)
            {
                m = source.GetLength(0);
                n = source.GetLength(1);
                table = new double[m, n + m - 1];
                basis = new List<int>();

                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < table.GetLength(1); j++)
                    {
                        if (j < n)
                            table[i, j] = source[i, j];
                        else
                            table[i, j] = 0;
                    }

                    if ((n + i) < table.GetLength(1))
                    {
                        table[i, n + i] = 1;
                        basis.Add(n + i);
                    }
                }

                n = table.GetLength(1);
            }

            public double[,] Calculate(double[] result)
            {
                int mainCol, mainRow;

                while (!IsItEnd())
                {
                    mainCol = findMainCol();
                    mainRow = findMainRow(mainCol);
                    basis[mainRow] = mainCol;

                    double[,] new_table = new double[m, n];

                    for (int j = 0; j < n; j++)
                        new_table[mainRow, j] = table[mainRow, j] / table[mainRow, mainCol];

                    for (int i = 0; i < m; i++)
                    {
                        if (i == mainRow)
                            continue;

                        for (int j = 0; j < n; j++)
                            new_table[i, j] = table[i, j] - table[i, mainCol] * new_table[mainRow, j];
                    }
                    table = new_table;
                }

                for (int i = 0; i < result.Length; i++)
                {
                    int k = basis.IndexOf(i + 1);
                    if (k != -1)
                        result[i] = table[k, 0];
                    else
                        result[i] = 0;
                }

                return table;
            }

            private bool IsItEnd()
            {
                bool flag = true;

                for (int j = 1; j < n; j++)
                {
                    if (table[m - 1, j] < 0)
                    {
                        flag = false;
                        break;
                    }
                }

                return flag;
            }

            private int findMainCol()
            {
                int mainCol = 1;

                for (int j = 2; j < n; j++)
                    if (table[m - 1, j] < table[m - 1, mainCol])
                        mainCol = j;

                return mainCol;
            }

            private int findMainRow(int mainCol)
            {
                int mainRow = 0;

                for (int i = 0; i < m - 1; i++)
                    if (table[i, mainCol] > 0)
                    {
                        mainRow = i;
                        break;
                    }

                for (int i = mainRow + 1; i < m - 1; i++)
                    if ((table[i, mainCol] > 0) && ((table[i, 0] / table[i, mainCol]) < (table[mainRow, 0] / table[mainRow, mainCol])))
                        mainRow = i;

                return mainRow;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
