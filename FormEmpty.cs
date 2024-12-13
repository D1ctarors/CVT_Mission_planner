using MissionPlanner;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace YourNamespace
{
    // Предполагается, что есть класс CurrentState с нужными полями, например:
    // public class CurrentState
    // {
    //     public float h_plant_output_voltag; // Вольтаж
    //     public float h_plant_temp1;         // Температура
    //     public float h_plant_pressure;      // Давление
    //     public float h_plant_fan;           // ШИМ вентиляторов
    //     public float h_plant_runtime;       // Время работы
    //     public float h_plant_te_voltage;    // ТЭ Вольтаж
    //     public float h_plant_current;       // Потребление (А)
    //     public string errors;               // Строка ошибок (если есть)
    // }

    public class FormEmpty : Form
    {
        private CurrentState currentState;

        private Label lblVoltage;
        private Label lblTemperature;
        private Label lblPressure;
        private Label lblFanPWM;
        private Label lblWorkTime;
        private Label lblTEVoltage;
        private Label lblCurrent;
        private Label lblErrorList;

        private Timer updateTimer;

        public FormEmpty(CurrentState state)
        {
            currentState = state;

            // Общие настройки формы
            this.Text = "Мониторинг параметров";
            this.BackColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(800, 400);

            // Основная таблица: 2 столбца - слева параметры, справа ошибки
            TableLayoutPanel mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Black,
                Padding = new Padding(20)
            };
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            this.Controls.Add(mainTable);

            // Левая панель для параметров
            FlowLayoutPanel leftPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                AutoScroll = true
            };
            mainTable.Controls.Add(leftPanel, 0, 0);

            // Функция для создания однотипных лейблов параметров
            Label CreateParamLabel(string text)
            {
                return new Label
                {
                    Text = text,
                    ForeColor = Color.White,
                    BackColor = Color.Black,
                    Font = new Font("Arial", 14, FontStyle.Regular),
                    AutoSize = true,
                    Margin = new Padding(0, 10, 0, 0)
                };
            }

            lblVoltage = CreateParamLabel("Напряжение: ...");
            lblTemperature = CreateParamLabel("Температура: ...");
            lblPressure = CreateParamLabel("Давление: ...");
            lblFanPWM = CreateParamLabel("ШИМ вентиляторов: ...");
            lblWorkTime = CreateParamLabel("Время работы: ...");
            lblTEVoltage = CreateParamLabel("ТЭ Вольтаж: ...");
            lblCurrent = CreateParamLabel("Потребление (А): ...");

            leftPanel.Controls.Add(lblVoltage);
            leftPanel.Controls.Add(lblTemperature);
            leftPanel.Controls.Add(lblPressure);
            leftPanel.Controls.Add(lblFanPWM);
            leftPanel.Controls.Add(lblWorkTime);
            leftPanel.Controls.Add(lblTEVoltage);
            leftPanel.Controls.Add(lblCurrent);

            // Правая панель (Ошибки)
            Panel rightPanel = new Panel
            {
                BackColor = Color.Black,
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            mainTable.Controls.Add(rightPanel, 1, 0);

            Label lblErrorsTitle = new Label
            {
                Text = "Ошибки:",
                ForeColor = Color.White,
                BackColor = Color.Black,
                Font = new Font("Arial", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(0, 0)
            };
            rightPanel.Controls.Add(lblErrorsTitle);

            lblErrorList = new Label
            {
                Text = "Нет ошибок",
                ForeColor = Color.White,
                BackColor = Color.Black,
                Font = new Font("Arial", 12, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(0, 40)
            };
            rightPanel.Controls.Add(lblErrorList);

            // Таймер для обновления данных
            updateTimer = new Timer();
            updateTimer.Interval = 1000; // обновление раз в секунду
            updateTimer.Tick += UpdateLabels;
            updateTimer.Start();
        }

        private void UpdateLabels(object sender, EventArgs e)
        {
            // Обновляем значения на форме в соответствии с currentState
            lblTemperature.Text = $"Температура: {currentState.h_plant_temp1:F1} °C";
            lblPressure.Text = $"Давление: {currentState.h_plant_pressure:F1} атм";
            lblFanPWM.Text = $"ШИМ вентиляторов: {currentState.h_plant_fan:F0}%";
            lblWorkTime.Text = $"Время работы: {currentState.h_plant_runtime:F0} сек";
            lblTEVoltage.Text = $"ТЭ Вольтаж: {currentState.h_plant_te_voltage:F2} В";
            lblCurrent.Text = $"Потребление (А): {currentState.h_plant_current:F2} А";

        }
    }
}
