using System.Drawing;
using System.Windows.Forms;

namespace Velocity.Frontend;

partial class LoginForm
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
        components = new System.ComponentModel.Container();
        textBox1 = new TextBox();
        label1 = new Label();
        label2 = new Label();
        panel1 = new Panel();
        label3 = new Label();
        button1 = new Button();
        textBox2 = new TextBox();
        button2 = new Button();
        errorProvider1 = new ErrorProvider(components);
        panel1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
        SuspendLayout();
        // 
        // textBox1
        // 
        textBox1.Font = new Font("Segoe UI Variable Display", 10.8F, FontStyle.Regular, GraphicsUnit.Point);
        textBox1.Location = new Point(183, 135);
        textBox1.Name = "textBox1";
        textBox1.Size = new Size(229, 31);
        textBox1.TabIndex = 0;
        textBox1.TextChanged += LoginForm_Load;
        textBox1.Validating += textBox1_Validating;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(75, 141);
        label1.Name = "label1";
        label1.Size = new Size(85, 20);
        label1.TabIndex = 1;
        label1.Text = "User Name:";
        label1.Click += label1_Click;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(87, 207);
        label2.Name = "label2";
        label2.Size = new Size(73, 20);
        label2.TabIndex = 3;
        label2.Text = "Password:";
        // 
        // panel1
        // 
        panel1.BackColor = Color.RoyalBlue;
        panel1.Controls.Add(label3);
        panel1.Location = new Point(12, 13);
        panel1.Name = "panel1";
        panel1.Size = new Size(464, 83);
        panel1.TabIndex = 4;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Font = new Font("Segoe UI Variable Display", 16.2F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point);
        label3.ForeColor = Color.White;
        label3.Location = new Point(98, 22);
        label3.Name = "label3";
        label3.Size = new Size(258, 37);
        label3.TabIndex = 0;
        label3.Text = "LOGIN - VELOCITY";
        // 
        // button1
        // 
        button1.Location = new Point(12, 277);
        button1.Name = "button1";
        button1.Size = new Size(227, 49);
        button1.TabIndex = 5;
        button1.Text = "Login";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // textBox2
        // 
        textBox2.Font = new Font("Segoe UI Variable Display", 10.8F, FontStyle.Regular, GraphicsUnit.Point);
        textBox2.Location = new Point(183, 201);
        textBox2.Name = "textBox2";
        textBox2.Size = new Size(229, 31);
        textBox2.TabIndex = 7;
        // 
        // button2
        // 
        button2.Location = new Point(249, 277);
        button2.Name = "button2";
        button2.Size = new Size(227, 49);
        button2.TabIndex = 8;
        button2.Text = "Exit";
        button2.UseVisualStyleBackColor = true;
        button2.Click += button2_Click;
        // 
        // errorProvider1
        // 
        errorProvider1.ContainerControl = this;
        // 
        // LoginForm
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        CancelButton = button2;
        ClientSize = new Size(488, 340);
        Controls.Add(button2);
        Controls.Add(textBox2);
        Controls.Add(button1);
        Controls.Add(panel1);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(textBox1);
        MaximizeBox = false;
        Name = "LoginForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Login";
        Load += LoginForm_Load;
        panel1.ResumeLayout(false);
        panel1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TextBox textBox1;
    private Label label1;
    private Label label2;
    private Panel panel1;
    private Label label3;
    private Button button1;
    private TextBox textBox2;
    private Button button2;
    private ErrorProvider errorProvider1;
}
