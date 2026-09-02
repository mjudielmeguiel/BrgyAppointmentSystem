<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmCreateAppointment
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCreateAppointment))
        Me.btnClose = New System.Windows.Forms.Button()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.Label63 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnSubmit = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboRequestType = New System.Windows.Forms.ComboBox()
        Me.cboRequestFor = New System.Windows.Forms.ComboBox()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cboDepartment = New System.Windows.Forms.ComboBox()
        Me.lblControlNo = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtPurpose = New System.Windows.Forms.TextBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSelectUser = New System.Windows.Forms.Button()
        Me.picUserProfile = New System.Windows.Forms.PictureBox()
        Me.picAuthLetter = New System.Windows.Forms.PictureBox()
        Me.picRepID = New System.Windows.Forms.PictureBox()
        Me.lblAuthLetter = New System.Windows.Forms.Label()
        Me.lblRepID = New System.Windows.Forms.Label()
        Me.lblRepName = New System.Windows.Forms.Label()
        Me.txtNameOfRepresentative = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        CType(Me.picUserProfile, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picAuthLetter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picRepID, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.SystemColors.Control
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnClose.FlatAppearance.BorderSize = 0
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.DarkBlue
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.Location = New System.Drawing.Point(652, 12)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(53, 39)
        Me.btnClose.TabIndex = 157
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'Label50
        '
        Me.Label50.AutoSize = True
        Me.Label50.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label50.ForeColor = System.Drawing.Color.Navy
        Me.Label50.Location = New System.Drawing.Point(8, 19)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(132, 22)
        Me.Label50.TabIndex = 499
        Me.Label50.Text = "Create Request"
        '
        'Label63
        '
        Me.Label63.AutoSize = True
        Me.Label63.Font = New System.Drawing.Font("Microsoft YaHei UI Light", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label63.Location = New System.Drawing.Point(165, 168)
        Me.Label63.Name = "Label63"
        Me.Label63.Size = New System.Drawing.Size(80, 19)
        Me.Label63.TabIndex = 502
        Me.Label63.Text = "Request For"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Navy
        Me.Label1.Location = New System.Drawing.Point(12, 101)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(169, 22)
        Me.Label1.TabIndex = 503
        Me.Label1.Text = "Service Information"
        '
        'btnSubmit
        '
        Me.btnSubmit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSubmit.BackColor = System.Drawing.Color.MidnightBlue
        Me.btnSubmit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSubmit.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSubmit.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnSubmit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSubmit.Location = New System.Drawing.Point(12, 617)
        Me.btnSubmit.Name = "btnSubmit"
        Me.btnSubmit.Size = New System.Drawing.Size(693, 37)
        Me.btnSubmit.TabIndex = 507
        Me.btnSubmit.Text = "Submit"
        Me.btnSubmit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnSubmit.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft YaHei UI Light", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 289)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(88, 19)
        Me.Label2.TabIndex = 509
        Me.Label2.Text = "Request Type"
        '
        'cboRequestType
        '
        Me.cboRequestType.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cboRequestType.Font = New System.Drawing.Font("Microsoft YaHei UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRequestType.FormattingEnabled = True
        Me.cboRequestType.Location = New System.Drawing.Point(12, 311)
        Me.cboRequestType.Name = "cboRequestType"
        Me.cboRequestType.Size = New System.Drawing.Size(313, 28)
        Me.cboRequestType.TabIndex = 508
        '
        'cboRequestFor
        '
        Me.cboRequestFor.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cboRequestFor.Font = New System.Drawing.Font("Microsoft YaHei UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRequestFor.FormattingEnabled = True
        Me.cboRequestFor.Location = New System.Drawing.Point(169, 190)
        Me.cboRequestFor.Name = "cboRequestFor"
        Me.cboRequestFor.Size = New System.Drawing.Size(536, 28)
        Me.cboRequestFor.TabIndex = 583
        '
        'txtName
        '
        Me.txtName.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtName.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtName.Location = New System.Drawing.Point(169, 126)
        Me.txtName.Name = "txtName"
        Me.txtName.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal
        Me.txtName.Size = New System.Drawing.Size(490, 28)
        Me.txtName.TabIndex = 584
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft YaHei UI Light", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(215, 104)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(45, 19)
        Me.Label3.TabIndex = 585
        Me.Label3.Text = "Name"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft YaHei UI Light", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(331, 289)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(80, 19)
        Me.Label4.TabIndex = 590
        Me.Label4.Text = "Department"
        '
        'cboDepartment
        '
        Me.cboDepartment.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cboDepartment.Font = New System.Drawing.Font("Microsoft YaHei UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDepartment.FormattingEnabled = True
        Me.cboDepartment.Location = New System.Drawing.Point(331, 311)
        Me.cboDepartment.Name = "cboDepartment"
        Me.cboDepartment.Size = New System.Drawing.Size(374, 28)
        Me.cboDepartment.TabIndex = 589
        '
        'lblControlNo
        '
        Me.lblControlNo.AutoSize = True
        Me.lblControlNo.Font = New System.Drawing.Font("Microsoft YaHei UI Light", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblControlNo.Location = New System.Drawing.Point(543, 73)
        Me.lblControlNo.Name = "lblControlNo"
        Me.lblControlNo.Size = New System.Drawing.Size(108, 19)
        Me.lblControlNo.TabIndex = 593
        Me.lblControlNo.Text = "Control Number"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft YaHei UI Light", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(165, 226)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(58, 19)
        Me.Label6.TabIndex = 595
        Me.Label6.Text = "Purpose"
        '
        'txtPurpose
        '
        Me.txtPurpose.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.txtPurpose.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPurpose.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPurpose.Location = New System.Drawing.Point(169, 248)
        Me.txtPurpose.Name = "txtPurpose"
        Me.txtPurpose.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal
        Me.txtPurpose.Size = New System.Drawing.Size(536, 28)
        Me.txtPurpose.TabIndex = 594
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft YaHei UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ForeColor = System.Drawing.Color.DarkBlue
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.Location = New System.Drawing.Point(12, 660)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(693, 37)
        Me.btnCancel.TabIndex = 597
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnSelectUser
        '
        Me.btnSelectUser.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSelectUser.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnSelectUser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnSelectUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSelectUser.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectUser.ForeColor = System.Drawing.Color.DarkBlue
        Me.btnSelectUser.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSelectUser.Location = New System.Drawing.Point(665, 126)
        Me.btnSelectUser.Name = "btnSelectUser"
        Me.btnSelectUser.Size = New System.Drawing.Size(40, 27)
        Me.btnSelectUser.TabIndex = 598
        Me.btnSelectUser.Text = "..."
        Me.btnSelectUser.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnSelectUser.UseVisualStyleBackColor = False
        '
        'picUserProfile
        '
        Me.picUserProfile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picUserProfile.Image = CType(resources.GetObject("picUserProfile.Image"), System.Drawing.Image)
        Me.picUserProfile.Location = New System.Drawing.Point(12, 126)
        Me.picUserProfile.Name = "picUserProfile"
        Me.picUserProfile.Size = New System.Drawing.Size(151, 150)
        Me.picUserProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picUserProfile.TabIndex = 601
        Me.picUserProfile.TabStop = False
        '
        'picAuthLetter
        '
        Me.picAuthLetter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picAuthLetter.Location = New System.Drawing.Point(12, 379)
        Me.picAuthLetter.Name = "picAuthLetter"
        Me.picAuthLetter.Size = New System.Drawing.Size(151, 216)
        Me.picAuthLetter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picAuthLetter.TabIndex = 602
        Me.picAuthLetter.TabStop = False
        '
        'picRepID
        '
        Me.picRepID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picRepID.Location = New System.Drawing.Point(169, 379)
        Me.picRepID.Name = "picRepID"
        Me.picRepID.Size = New System.Drawing.Size(151, 216)
        Me.picRepID.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picRepID.TabIndex = 607
        Me.picRepID.TabStop = False
        '
        'lblAuthLetter
        '
        Me.lblAuthLetter.AutoSize = True
        Me.lblAuthLetter.Font = New System.Drawing.Font("Microsoft YaHei UI Light", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAuthLetter.Location = New System.Drawing.Point(8, 357)
        Me.lblAuthLetter.Name = "lblAuthLetter"
        Me.lblAuthLetter.Size = New System.Drawing.Size(117, 19)
        Me.lblAuthLetter.TabIndex = 608
        Me.lblAuthLetter.Text = "Autorization letter"
        '
        'lblRepID
        '
        Me.lblRepID.AutoSize = True
        Me.lblRepID.Font = New System.Drawing.Font("Microsoft YaHei UI Light", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRepID.Location = New System.Drawing.Point(165, 357)
        Me.lblRepID.Name = "lblRepID"
        Me.lblRepID.Size = New System.Drawing.Size(144, 19)
        Me.lblRepID.TabIndex = 609
        Me.lblRepID.Text = "Representative Valid ID"
        '
        'lblRepName
        '
        Me.lblRepName.AutoSize = True
        Me.lblRepName.Font = New System.Drawing.Font("Microsoft YaHei UI Light", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRepName.Location = New System.Drawing.Point(326, 357)
        Me.lblRepName.Name = "lblRepName"
        Me.lblRepName.Size = New System.Drawing.Size(131, 19)
        Me.lblRepName.TabIndex = 611
        Me.lblRepName.Text = "RepresentativeName"
        '
        'txtNameOfRepresentative
        '
        Me.txtNameOfRepresentative.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.txtNameOfRepresentative.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNameOfRepresentative.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNameOfRepresentative.Location = New System.Drawing.Point(326, 379)
        Me.txtNameOfRepresentative.Name = "txtNameOfRepresentative"
        Me.txtNameOfRepresentative.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal
        Me.txtNameOfRepresentative.Size = New System.Drawing.Size(333, 28)
        Me.txtNameOfRepresentative.TabIndex = 610
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.Color.DarkBlue
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button1.Location = New System.Drawing.Point(665, 379)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(40, 27)
        Me.Button1.TabIndex = 612
        Me.Button1.Text = "..."
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button1.UseVisualStyleBackColor = False
        '
        'frmCreateAppointment
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(717, 709)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.lblRepName)
        Me.Controls.Add(Me.txtNameOfRepresentative)
        Me.Controls.Add(Me.lblRepID)
        Me.Controls.Add(Me.lblAuthLetter)
        Me.Controls.Add(Me.picRepID)
        Me.Controls.Add(Me.picAuthLetter)
        Me.Controls.Add(Me.picUserProfile)
        Me.Controls.Add(Me.btnSelectUser)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtPurpose)
        Me.Controls.Add(Me.lblControlNo)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cboDepartment)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.cboRequestFor)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cboRequestType)
        Me.Controls.Add(Me.btnSubmit)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label63)
        Me.Controls.Add(Me.Label50)
        Me.Controls.Add(Me.btnClose)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmCreateAppointment"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmCreateAppointment"
        CType(Me.picUserProfile, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picAuthLetter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picRepID, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnClose As Button
    Friend WithEvents Label50 As Label
    Friend WithEvents Label63 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents btnSubmit As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents cboRequestType As ComboBox
    Friend WithEvents cboRequestFor As ComboBox
    Friend WithEvents txtName As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents cboDepartment As ComboBox
    Friend WithEvents lblControlNo As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents txtPurpose As TextBox
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnSelectUser As Button
    Friend WithEvents picUserProfile As PictureBox
    Friend WithEvents picAuthLetter As PictureBox
    Friend WithEvents picRepID As PictureBox
    Friend WithEvents lblAuthLetter As Label
    Friend WithEvents lblRepID As Label
    Friend WithEvents lblRepName As Label
    Friend WithEvents txtNameOfRepresentative As TextBox
    Friend WithEvents Button1 As Button
End Class
