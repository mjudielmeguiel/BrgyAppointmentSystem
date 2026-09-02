<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmResidence_Records
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmResidence_Records))
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.btnPrevMonth = New System.Windows.Forms.Button()
        Me.btnNextMonth = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblWelcomeUser = New System.Windows.Forms.Label()
        Me.lblDateTime = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.lblTotalRecords = New System.Windows.Forms.Label()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dgvResidences = New System.Windows.Forms.DataGridView()
        Me.btnCreateRequest = New System.Windows.Forms.Button()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel4.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.dgvResidences, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Timer1
        '
        '
        'btnPrevMonth
        '
        Me.btnPrevMonth.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrevMonth.BackColor = System.Drawing.SystemColors.Control
        Me.btnPrevMonth.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnPrevMonth.FlatAppearance.BorderSize = 0
        Me.btnPrevMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrevMonth.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrevMonth.ForeColor = System.Drawing.Color.DarkBlue
        Me.btnPrevMonth.Image = CType(resources.GetObject("btnPrevMonth.Image"), System.Drawing.Image)
        Me.btnPrevMonth.Location = New System.Drawing.Point(1225, 726)
        Me.btnPrevMonth.Name = "btnPrevMonth"
        Me.btnPrevMonth.Size = New System.Drawing.Size(53, 39)
        Me.btnPrevMonth.TabIndex = 612
        Me.btnPrevMonth.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPrevMonth.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnPrevMonth.UseVisualStyleBackColor = False
        '
        'btnNextMonth
        '
        Me.btnNextMonth.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNextMonth.BackColor = System.Drawing.SystemColors.Control
        Me.btnNextMonth.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnNextMonth.FlatAppearance.BorderSize = 0
        Me.btnNextMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNextMonth.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNextMonth.ForeColor = System.Drawing.Color.DarkBlue
        Me.btnNextMonth.Image = CType(resources.GetObject("btnNextMonth.Image"), System.Drawing.Image)
        Me.btnNextMonth.Location = New System.Drawing.Point(1301, 726)
        Me.btnNextMonth.Name = "btnNextMonth"
        Me.btnNextMonth.Size = New System.Drawing.Size(53, 39)
        Me.btnNextMonth.TabIndex = 611
        Me.btnNextMonth.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnNextMonth.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnNextMonth.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft YaHei UI", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.MidnightBlue
        Me.Label1.Location = New System.Drawing.Point(111, 108)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(177, 42)
        Me.Label1.TabIndex = 616
        Me.Label1.Text = "Welcome,"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblWelcomeUser
        '
        Me.lblWelcomeUser.AutoSize = True
        Me.lblWelcomeUser.Font = New System.Drawing.Font("Microsoft YaHei UI", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWelcomeUser.ForeColor = System.Drawing.Color.DarkRed
        Me.lblWelcomeUser.Location = New System.Drawing.Point(294, 108)
        Me.lblWelcomeUser.Name = "lblWelcomeUser"
        Me.lblWelcomeUser.Size = New System.Drawing.Size(103, 42)
        Me.lblWelcomeUser.TabIndex = 615
        Me.lblWelcomeUser.Text = "User!"
        Me.lblWelcomeUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblDateTime
        '
        Me.lblDateTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDateTime.AutoSize = True
        Me.lblDateTime.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateTime.ForeColor = System.Drawing.Color.Navy
        Me.lblDateTime.Location = New System.Drawing.Point(930, 108)
        Me.lblDateTime.Name = "lblDateTime"
        Me.lblDateTime.Size = New System.Drawing.Size(17, 22)
        Me.lblDateTime.TabIndex = 607
        Me.lblDateTime.Text = "-"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(67, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(125, 19)
        Me.Label2.TabIndex = 520
        Me.Label2.Text = "City of Muntinlupa"
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(11, 11)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(50, 41)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox2.TabIndex = 518
        Me.PictureBox2.TabStop = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(66, 11)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(153, 22)
        Me.Label9.TabIndex = 519
        Me.Label9.Text = "Barangay Putatan"
        '
        'Panel4
        '
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.Label2)
        Me.Panel4.Controls.Add(Me.PictureBox2)
        Me.Panel4.Controls.Add(Me.Label9)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1366, 60)
        Me.Panel4.TabIndex = 613
        '
        'lblTotalRecords
        '
        Me.lblTotalRecords.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotalRecords.AutoSize = True
        Me.lblTotalRecords.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalRecords.Location = New System.Drawing.Point(12, 723)
        Me.lblTotalRecords.Name = "lblTotalRecords"
        Me.lblTotalRecords.Size = New System.Drawing.Size(92, 19)
        Me.lblTotalRecords.TabIndex = 619
        Me.lblTotalRecords.Text = "Total Records"
        '
        'Panel7
        '
        Me.Panel7.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel7.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel7.Controls.Add(Me.btnRefresh)
        Me.Panel7.Controls.Add(Me.Panel2)
        Me.Panel7.Controls.Add(Me.Label5)
        Me.Panel7.Controls.Add(Me.dgvResidences)
        Me.Panel7.Controls.Add(Me.btnCreateRequest)
        Me.Panel7.Location = New System.Drawing.Point(12, 236)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(1342, 484)
        Me.Panel7.TabIndex = 620
        '
        'btnRefresh
        '
        Me.btnRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRefresh.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRefresh.Font = New System.Drawing.Font("Microsoft YaHei UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRefresh.ForeColor = System.Drawing.Color.DarkBlue
        Me.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRefresh.Location = New System.Drawing.Point(1195, 12)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(131, 37)
        Me.btnRefresh.TabIndex = 552
        Me.btnRefresh.Text = "refresh"
        Me.btnRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnRefresh.UseVisualStyleBackColor = False
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Panel2.Controls.Add(Me.txtSearch)
        Me.Panel2.Location = New System.Drawing.Point(0, 61)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1340, 38)
        Me.Panel2.TabIndex = 509
        '
        'txtSearch
        '
        Me.txtSearch.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSearch.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.Location = New System.Drawing.Point(15, 5)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal
        Me.txtSearch.Size = New System.Drawing.Size(254, 28)
        Me.txtSearch.TabIndex = 223
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft YaHei UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.MidnightBlue
        Me.Label5.Location = New System.Drawing.Point(9, 12)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(240, 31)
        Me.Label5.TabIndex = 188
        Me.Label5.Text = "Residence Records"
        '
        'dgvResidences
        '
        Me.dgvResidences.AllowUserToAddRows = False
        Me.dgvResidences.AllowUserToDeleteRows = False
        Me.dgvResidences.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvResidences.BackgroundColor = System.Drawing.SystemColors.ButtonFace
        Me.dgvResidences.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvResidences.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvResidences.Location = New System.Drawing.Point(0, 99)
        Me.dgvResidences.Name = "dgvResidences"
        Me.dgvResidences.ReadOnly = True
        Me.dgvResidences.Size = New System.Drawing.Size(1340, 383)
        Me.dgvResidences.TabIndex = 508
        '
        'btnCreateRequest
        '
        Me.btnCreateRequest.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCreateRequest.BackColor = System.Drawing.Color.Navy
        Me.btnCreateRequest.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnCreateRequest.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCreateRequest.Font = New System.Drawing.Font("Microsoft YaHei UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCreateRequest.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnCreateRequest.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCreateRequest.Location = New System.Drawing.Point(1058, 12)
        Me.btnCreateRequest.Name = "btnCreateRequest"
        Me.btnCreateRequest.Size = New System.Drawing.Size(131, 37)
        Me.btnCreateRequest.TabIndex = 506
        Me.btnCreateRequest.Text = "Create new"
        Me.btnCreateRequest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnCreateRequest.UseVisualStyleBackColor = False
        '
        'frmResidence_Records
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1366, 768)
        Me.Controls.Add(Me.lblDateTime)
        Me.Controls.Add(Me.Panel7)
        Me.Controls.Add(Me.lblTotalRecords)
        Me.Controls.Add(Me.btnPrevMonth)
        Me.Controls.Add(Me.btnNextMonth)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblWelcomeUser)
        Me.Controls.Add(Me.Panel4)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmResidence_Records"
        Me.Text = "frmResidence_Records"
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel7.ResumeLayout(False)
        Me.Panel7.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.dgvResidences, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Timer1 As Timer
    Friend WithEvents btnPrevMonth As Button
    Friend WithEvents btnNextMonth As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents lblWelcomeUser As Label
    Friend WithEvents lblDateTime As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Panel4 As Panel
    Friend WithEvents lblTotalRecords As Label
    Friend WithEvents Panel7 As Panel
    Friend WithEvents btnRefresh As Button
    Friend WithEvents Panel2 As Panel
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents dgvResidences As DataGridView
    Friend WithEvents btnCreateRequest As Button
End Class
