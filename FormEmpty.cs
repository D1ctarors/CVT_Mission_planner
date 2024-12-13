using MissionPlanner;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace YourNamespace
{
    // Предполагается, что есть класс CurrentState с нужными свойствами:
    // public class CurrentState
    // {
    //     public float h_plant_temp1 { get; set; }
    //     public float h_plant_temp2 { get; set; }
    //     public float h_plant_pressure { get; set; }
    //     public float h_plant_current { get; set; }
    //     public float h_plant_battery_voltage { get; set; }
    //     public float h_plant_output_voltage { get; set; }
    //     public float h_plant_te_voltage { get; set; }
    //     public int   h_plant_runtime { get; set; }
    //     public int   h_plant_fan { get; set; }
    //     public int   h_plant_counter { get; set; }
    //     public int   h_plant_status { get; set; }
    //     public string h_plant_errors { get; set; }
    // }

    public class FormEmpty : Form
    {
        private CurrentState currentState;

        // Лейблы для параметров
        private Label lblTemp1;
        private Label lblTemp2;
        private Label lblPressure;
        private Label lblCurrent;
        private Label lblBatteryVoltage;
        private Label lblOutputVoltage;
        private Label lblTEVoltage;
        private Label lblRuntime;
        private Label lblFan;
        private Label lblCounter;
        private Label lblStatus;

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
            this.Size = new Size(1000, 600);

            // Создаём основную таблицу: 3 столбца
            // 1-й столбец: статус (для примера - зелёные кнопки)
            // 2-й столбец: показания (наши параметры)
            // 3-й столбец: ошибки
            TableLayoutPanel mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.Black,
                Padding = new Padding(20)
            };
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.Controls.Add(mainTable);

            // Левая панель - статусы (для примерного сходства с макетом)
            FlowLayoutPanel leftPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                AutoScroll = true
            };
            mainTable.Controls.Add(leftPanel, 0, 0);

            // Пример статусов (зелёные кнопки)
            string[] statusTexts = { "Авария", "Запуск", "Работа", "Нагрузка вкл.", "Короткое ТЭ", "Клапан подачи", "Клапан сброса" };
            foreach (var text in statusTexts)
            {
                Button btn = new Button
                {
                    Text = text,
                    ForeColor = Color.White,
                    BackColor = Color.Green,
                    FlatStyle = FlatStyle.Flat,
                    Width = 150,
                    Height = 40,
                    Font = new Font("Arial", 12, FontStyle.Regular),
                    Margin = new Padding(0, 10, 0, 0)
                };
                btn.FlatAppearance.BorderSize = 0;
                leftPanel.Controls.Add(btn);
            }

            // Центральная панель - Показания
            FlowLayoutPanel middlePanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                AutoScroll = true,
                Padding = new Padding(20, 0, 20, 0)
            };
            mainTable.Controls.Add(middlePanel, 1, 0);

            // Заголовок "Показания"
            Label lblDataTitle = new Label
            {
                Text = "Показания",
                ForeColor = Color.White,
                BackColor = Color.Black,
                Font = new Font("Arial", 16, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 20)
            };
            middlePanel.Controls.Add(lblDataTitle);

            // Функция создания лейблов для параметров
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

            // Создаём лейблы для всех нужных полей
            lblTemp1 = CreateParamLabel("Температура 1: ...");
            lblTemp2 = CreateParamLabel("Температура 2: ...");
            lblPressure = CreateParamLabel("Давление: ...");
            lblCurrent = CreateParamLabel("Потребление (А): ...");
            lblBatteryVoltage = CreateParamLabel("Напряжение батареи: ...");
            lblOutputVoltage = CreateParamLabel("Выходное напряжение: ...");
            lblTEVoltage = CreateParamLabel("ТЭ Вольтаж: ...");
            lblRuntime = CreateParamLabel("Время работы: ...");
            lblFan = CreateParamLabel("ШИМ вентиляторов: ...");
            lblCounter = CreateParamLabel("Счётчик: ...");
            lblStatus = CreateParamLabel("Статус: ...");

            // Добавляем их в центральную панель
            middlePanel.Controls.Add(lblTemp1);
            middlePanel.Controls.Add(lblTemp2);
            middlePanel.Controls.Add(lblPressure);
            middlePanel.Controls.Add(lblCurrent);
            middlePanel.Controls.Add(lblBatteryVoltage);
            middlePanel.Controls.Add(lblOutputVoltage);
            middlePanel.Controls.Add(lblTEVoltage);
            middlePanel.Controls.Add(lblRuntime);
            middlePanel.Controls.Add(lblFan);
            middlePanel.Controls.Add(lblCounter);
            middlePanel.Controls.Add(lblStatus);

            // Правая панель (Ошибки)
            FlowLayoutPanel rightPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                AutoScroll = true,
                Padding = new Padding(20, 0, 0, 0)
            };
            mainTable.Controls.Add(rightPanel, 2, 0);

            Label lblErrorsTitle = new Label
            {
                Text = "Ошибки:",
                ForeColor = Color.White,
                BackColor = Color.Black,
                Font = new Font("Arial", 16, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 20)
            };
            rightPanel.Controls.Add(lblErrorsTitle);

            lblErrorList = new Label
            {
                Text = "Нет ошибок",
                ForeColor = Color.White,
                BackColor = Color.Black,
                Font = new Font("Arial", 12, FontStyle.Regular),
                AutoSize = true
            };
            rightPanel.Controls.Add(lblErrorList);

            // Таймер для обновления данных
            updateTimer = new Timer();
            updateTimer.Interval = 1000; // Обновление раз в секунду
            updateTimer.Tick += UpdateLabels;
            updateTimer.Start();
        }

        private void UpdateLabels(object sender, EventArgs e)
        {
            // Обновляем значения
            lblTemp1.Text = $"Температура 1: {currentState.h_plant_temp1:F1} °C";
            lblTemp2.Text = $"Температура 2: {currentState.h_plant_temp2:F1} °C";
            lblPressure.Text = $"Давление: {currentState.h_plant_pressure:F1} атм";
            lblCurrent.Text = $"Потребление (А): {currentState.h_plant_current:F2} А";
            lblBatteryVoltage.Text = $"Напряжение батареи: {currentState.h_plant_battery_voltage:F2} В";
            lblOutputVoltage.Text = $"Выходное напряжение: {currentState.h_plant_output_voltage:F2} В";
            lblTEVoltage.Text = $"ТЭ Вольтаж: {currentState.h_plant_te_voltage:F2} В";
            lblRuntime.Text = $"Время работы: {currentState.h_plant_runtime} сек";
            lblFan.Text = $"ШИМ вентиляторов: {currentState.h_plant_fan}%";
            lblCounter.Text = $"Счётчик: {currentState.h_plant_counter}";
            lblStatus.Text = $"Статус: {currentState.h_plant_status}";

        }
    }
}
