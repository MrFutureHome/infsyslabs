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
        private const int Padding = 10; // Отступы между элементами
        private const int ElementHeight = 25;
        private const int MinWidth = 50;
        private double[,] simplexTable;

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

            Label maximizationLabel = new Label
            {
                Text = $"-> max",
                AutoSize = true,
                Location = new Point(numVariables * (fieldWidth + Padding * 2) - 5, 44) // Метка справа от TextBox
            };

            variablesPanel.Controls.Add(maximizationLabel);

            // Радиокнопки для выбора типа задачи (максимизация/минимизация)
            /*RadioButton maxRadioButton = new RadioButton
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
            variablesPanel.Controls.Add(minRadioButton);*/
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

            textBoxArray = new TextBox[numConstraints, numVariables + 1];

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
                    textBoxArray[i, j] = variableTextBox;
                    constraintsPanel.Controls.Add(variableLabel);
                }

                TextBox rightSide = new TextBox
                {
                    Width = fieldWidth,
                    Location = new Point((numVariables + 1) * (fieldWidth + Padding * 2), yPosition)
                };

                int centerXPosition = (rightSide.Location.X + rightSide.Width + (numVariables * (fieldWidth + Padding * 2))) / 2;

                Label lessEqualLabel = new Label
                {
                    Text = "<=",
                    AutoSize = true,
                    Location = new Point(centerXPosition - 50, yPosition + 4)
                };

                constraintsPanel.Controls.Add(lessEqualLabel);
                constraintsPanel.Controls.Add(rightSide);
            }
        }

        private double[,] ReadMatrixValues()
        {
            int rows = textBoxArray.GetLength(0);
            int cols = textBoxArray.GetLength(1);
            double[,] matrixValues = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (textBoxArray[i, j] != null)
                    {
                        if (double.TryParse(textBoxArray[i, j].Text, out double result))
                        {
                            matrixValues[i, j] = result;
                        }
                        else
                        {
                            MessageBox.Show($"Некорректное значение в ячейке ({i + 1}, {j + 1}). Введите число.");
                            return null; // Возвращаем null при ошибке
                        }
                    }
                }
            }
            return matrixValues;
        }

        private void SolveSimplexMethod()
        {
            double[,] matrixValues = ReadMatrixValues();
            if (matrixValues == null) return; // Проверка на наличие ошибок ввода

            int numVariables = matrixValues.GetLength(1) - 1; // Последний столбец - это правые части ограничений
            int numConstraints = matrixValues.GetLength(0);

            double[,] A = new double[numConstraints, numVariables];
            double[] b = new double[numConstraints];

            for (int i = 0; i < numConstraints; i++)
            {
                for (int j = 0; j < numVariables; j++)
                {
                    A[i, j] = matrixValues[i, j]; // Коэффициенты ограничений
                }
                b[i] = matrixValues[i, numVariables]; // Правые части ограничений
            }

            // Заполнение целевой функции: max Z = c^T * x
            double[] c = new double[numVariables];
            for (int j = 0; j < numVariables; j++)
            {
                c[j] = (j == 0) ? 2 : 3; // Коэффициенты целевой функции: 2x1 + 3x2
            }

            double[] solution = SimplexAlgorithm(A, b, c, numVariables, numConstraints);

            if (solution != null)
            {
                // Вывод решения
                MessageBox.Show($"Решение: x1 = {solution[0]}, x2 = {solution[1]}");
            }
        }

        private double[] SimplexAlgorithm(double[,] A, double[] b, double[] c, int numVariables, int numConstraints)
        {
            // Создаем симплекс-таблицу
            int totalVariables = numVariables + numConstraints;
            double[,] simplexTable = new double[numConstraints + 1, totalVariables + 1];

            // Заполняем симплекс-таблицу
            for (int i = 0; i < numConstraints; i++)
            {
                for (int j = 0; j < numVariables; j++)
                {
                    simplexTable[i, j] = A[i, j]; // Заполняем ограничения
                }
                simplexTable[i, totalVariables] = b[i]; // Правые части
            }

            // Заполняем целевую функцию (последняя строка)
            for (int j = 0; j < numVariables; j++)
            {
                simplexTable[numConstraints, j] = -c[j]; // Мы берем -c для максимизации
            }

            // Итерация симплекс-метода
            while (true)
            {
                // Находим входящий столбец (переменная, которая будет введена в базис)
                int pivotColumn = -1;
                for (int j = 0; j < totalVariables; j++)
                {
                    if (simplexTable[numConstraints, j] > 0)
                    {
                        pivotColumn = j;
                        break;
                    }
                }

                // Если нет положительных коэффициентов, выходим из цикла
                if (pivotColumn == -1)
                    break;

                // Находим выходящую строку (ограничение, которое будет исключено из базиса)
                int pivotRow = -1;
                double minRatio = double.MaxValue;

                for (int i = 0; i < numConstraints; i++)
                {
                    if (simplexTable[i, pivotColumn] > 0)
                    {
                        double ratio = simplexTable[i, totalVariables] / simplexTable[i, pivotColumn];
                        if (ratio < minRatio)
                        {
                            minRatio = ratio;
                            pivotRow = i;
                        }
                    }
                }

                // Если нет выходящей строки, выходим из цикла
                if (pivotRow == -1)
                    break;

                // Обновление симплекс-таблицы
                double pivotValue = simplexTable[pivotRow, pivotColumn];
                for (int j = 0; j <= totalVariables; j++)
                {
                    simplexTable[pivotRow, j] /= pivotValue; // Делим на опорный элемент
                }

                for (int i = 0; i <= numConstraints; i++)
                {
                    if (i != pivotRow)
                    {
                        double factor = simplexTable[i, pivotColumn];
                        for (int j = 0; j <= totalVariables; j++)
                        {
                            simplexTable[i, j] -= factor * simplexTable[pivotRow, j];
                        }
                    }
                }
            }

            double[] result = new double[numVariables];
            for (int j = 0; j < numVariables; j++)
            {
                result[j] = simplexTable[j, totalVariables]; // Значения переменных
            }

            return result;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            SolveSimplexMethod();
        }
    }
}
