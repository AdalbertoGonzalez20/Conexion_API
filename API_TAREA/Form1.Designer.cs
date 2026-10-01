namespace API_TAREA
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblIngresar_URL = new Label();
            txtUrl = new TextBox();
            btnConsultar = new Button();
            rbtResultado = new RichTextBox();
            btnTxt = new Button();
            btnCsv = new Button();
            btnJson = new Button();
            SuspendLayout();
            // 
            // lblIngresar_URL
            // 
            lblIngresar_URL.AutoSize = true;
            lblIngresar_URL.Font = new Font("Segoe UI Semibold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblIngresar_URL.Location = new Point(12, 9);
            lblIngresar_URL.Name = "lblIngresar_URL";
            lblIngresar_URL.Size = new Size(142, 31);
            lblIngresar_URL.TabIndex = 0;
            lblIngresar_URL.Text = "Url de la API";
            lblIngresar_URL.Click += label1_Click;
            // 
            // txtUrl
            // 
            txtUrl.Location = new Point(160, 12);
            txtUrl.Name = "txtUrl";
            txtUrl.Size = new Size(408, 27);
            txtUrl.TabIndex = 1;
            // 
            // btnConsultar
            // 
            btnConsultar.Location = new Point(614, 12);
            btnConsultar.Name = "btnConsultar";
            btnConsultar.Size = new Size(94, 29);
            btnConsultar.TabIndex = 2;
            btnConsultar.Text = "Consultar";
            btnConsultar.UseVisualStyleBackColor = true;
            btnConsultar.Click += btnConsultar_Click;
            // 
            // rbtResultado
            // 
            rbtResultado.Location = new Point(12, 71);
            rbtResultado.Name = "rbtResultado";
            rbtResultado.Size = new Size(776, 315);
            rbtResultado.TabIndex = 3;
            rbtResultado.Text = "";
            rbtResultado.TextChanged += rbtResultado_TextChanged;
            // 
            // btnTxt
            // 
            btnTxt.Location = new Point(193, 392);
            btnTxt.Name = "btnTxt";
            btnTxt.Size = new Size(118, 29);
            btnTxt.TabIndex = 4;
            btnTxt.Text = "Guardar en Txt";
            btnTxt.UseVisualStyleBackColor = true;
            btnTxt.Click += btnTxt_Click;
            // 
            // btnCsv
            // 
            btnCsv.Location = new Point(317, 392);
            btnCsv.Name = "btnCsv";
            btnCsv.Size = new Size(117, 29);
            btnCsv.TabIndex = 5;
            btnCsv.Text = "Guardar en Csv";
            btnCsv.UseVisualStyleBackColor = true;
            btnCsv.Click += btnCsv_Click;
            // 
            // btnJson
            // 
            btnJson.Location = new Point(440, 392);
            btnJson.Name = "btnJson";
            btnJson.Size = new Size(151, 29);
            btnJson.TabIndex = 6;
            btnJson.Text = "Guardar en Json";
            btnJson.UseVisualStyleBackColor = true;
            btnJson.Click += btnJson_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.InactiveCaption;
            ClientSize = new Size(800, 450);
            Controls.Add(btnJson);
            Controls.Add(btnCsv);
            Controls.Add(btnTxt);
            Controls.Add(rbtResultado);
            Controls.Add(btnConsultar);
            Controls.Add(txtUrl);
            Controls.Add(lblIngresar_URL);
            ForeColor = Color.Black;
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblIngresar_URL;
        private TextBox txtUrl;
        private Button btnConsultar;
        private RichTextBox rbtResultado;
        private Button btnTxt;
        private Button btnCsv;
        private Button btnJson;
    }
}
