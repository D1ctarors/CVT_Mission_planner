using System;
using System.Drawing;
using System.Windows.Forms;

namespace YourNamespace
{
    public class FormEmpty : Form
    {
        public FormEmpty()
        {
            this.Text = "Пример дизайна";
            this.BackColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(800, 500);
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

            FlowLayoutPanel leftPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                Padding = new Padding(0, 0, 20, 0),
                AutoScroll = true
            };
            mainTable.Controls.Add(leftPanel, 0, 0);

            string[] statusTexts = { "Авария", "Запуск", "Работа", "Нагрузка вкл.", "Короткое ТЭ", "Клапан подачи", "Клапан сброса" };
            foreach (var text in statusTexts)
            {
                Button btn = new Button
                {
                    Text = text,
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(0, 128, 0), 
                    FlatStyle = FlatStyle.Flat,
                    Width = 150,
                    Height = 40,
                    Font = new Font("Arial", 12, FontStyle.Regular),
                    Margin = new Padding(0, 10, 0, 0)
                };
                btn.FlatAppearance.BorderSize = 0;
                leftPanel.Controls.Add(btn);
            }

            Panel middlePanel = new Panel
            {
                BackColor = Color.Black,
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 0, 20, 0)
            };
            mainTable.Controls.Add(middlePanel, 1, 0);

            Label lblData = new Label
            {
                ForeColor = Color.White,
                BackColor = Color.Black,
                Font = new Font("Consolas", 10, FontStyle.Regular),
                AutoSize = true,
                Text = "Ток\nU аккумулятора\nU выхода\nU та\nВремя работы\nДавление H2\nТемпература1\nТемпература2\nШим вентилятора\nCount seq"
            };
            middlePanel.Controls.Add(lblData);
            lblData.Location = new Point(0, 0);

            Panel h2Container = new Panel
            {
                BackColor = Color.Black,
                Size = new Size(60, 200),
                Location = new Point(200, 0)
            };
            middlePanel.Controls.Add(h2Container);

            Label h2Label = new Label
            {
                Text = "H2",
                ForeColor = Color.White,
                BackColor = Color.Black,
                AutoSize = true
            };
            h2Container.Controls.Add(h2Label);
            h2Label.Location = new Point((h2Container.Width - h2Label.Width) / 2, 0);

            Panel h2Level = new Panel
            {
                BackColor = Color.FromArgb(0, 150, 200),
                Size = new Size(40, 100), 
                Location = new Point(10, 30)
            };
            h2Container.Controls.Add(h2Level);

            Label h2HighValue = new Label
            {
                Text = "350",
                ForeColor = Color.White,
                BackColor = Color.Black,
                AutoSize = true,
                Location = new Point(10, 140)
            };
            Label h2LowValue = new Label
            {
                Text = "200",
                ForeColor = Color.White,
                BackColor = Color.Black,
                AutoSize = true,
                Location = new Point(10, 160)
            };
            h2Container.Controls.Add(h2HighValue);
            h2Container.Controls.Add(h2LowValue);

            FlowLayoutPanel rightPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                Padding = new Padding(20, 0, 0, 0),
                AutoScroll = true
            };
            mainTable.Controls.Add(rightPanel, 2, 0);

            string[] errorTexts = { "Низкий ЗДС аккумулятора", "Давление по датчику", "Низкое давление по СК", "Утечка водорода по СК", "ЗДС ТЭ", "Перегрев ТЭ" };
            foreach (var etext in errorTexts)
            {
                Button errBtn = new Button
                {
                    Text = etext,
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(139, 0, 0),
                    FlatStyle = FlatStyle.Flat,
                    Width = 200,
                    Height = 40,
                    Font = new Font("Arial", 12, FontStyle.Regular),
                    Margin = new Padding(0, 10, 0, 0)
                };
                errBtn.FlatAppearance.BorderSize = 0;
                rightPanel.Controls.Add(errBtn);
            }
        }
    }
}
