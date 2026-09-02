<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAppointmentDetails
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAppointmentDetails))
        Me.lblEmail = New System.Windows.Forms.Label()
        Me.cboServiceType = New System.Windows.Forms.ComboBox()
        Me.lblName = New System.Windows.Forms.Label()
        Me.lblPhone = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.lblDateSubmitted = New System.Windows.Forms.Label()
        Me.btnApprove = New System.Windows.Forms.Button()
        Me.picProfile = New System.Windows.Forms.PictureBox()
        Me.lblControlNo = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnReject = New System.Windows.Forms.Button()
        Me.picIDFront = New System.Windows.Forms.PictureBox()
        Me.picIDBack = New System.Windows.Forms.PictureBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.RtbAddress = New System.Windows.Forms.RichTextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.picRepID = New System.Windows.Forms.PictureBox()
        Me.picAuthLetter = New System.Windows.Forms.PictureBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.lblRequestFor = New System.Windows.Forms.Label()
        Me.lblRepresentativeName = New System.Windows.Forms.Label()
        CType(Me.picProfile, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picIDFront, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picIDBack, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picRepID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picAuthLetter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblEmail
        '
        Me.lblEmail.AutoSize = True
        Me.lblEmail.Font = New System.Drawing.Font("Microsoft YaHei UI Light", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmail.Location = New System.Drawing.Point(482, 489)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(15, 19)
        Me.lblEmail.TabIndex = 591
        Me.lblEmail.Text = "-"
        '
        'cboServiceType
        '
        Me.cboServiceType.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cboServiceType.Font = New System.Drawing.Font("Microsoft YaHei UI Light", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboServiceType.FormattingEnabled = True
        Me.cboServiceType.Location = New System.Drawing.Point(112, 392)
        Me.cboServiceType.Name = "cboServiceType"
        Me.cboServiceType.Size = New System.Drawing.Size(654, 27)
        Me.cboServiceType.TabIndex = 587
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Font = New System.Drawing.Font("Microsoft YaHei UI Light", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.Location = New System.Drawing.Point(108, 443)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(15, 19)
        Me.lblName.TabIndex = 592
        Me.lblName.Text = "-"
        '
        'lblPhone
        '
        Me.lblPhone.AutoSize = True
        Me.lblPhone.Font = New System.Drawing.Font("Microsoft YaHei UI Light", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPhone.Location = New System.Drawing.Point(79, 489)
        Me.lblPhone.Name = "lblPhone"
        Me.lblPhone.Size = New System.Drawing.Size(15, 19)
        Me.lblPhone.TabIndex = 594
        Me.lblPhone.Text = "-"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft YaHei UI Light", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(513, 80)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(15, 19)
        Me.lblStatus.TabIndex = 596
        Me.lblStatus.Text = "-"
        '
        'lblDateSubmitted
        '
        Me.lblDateSubmitted.AutoSize = True
        Me.lblDateSubmitted.Font = New System.Drawing.Font("Microsoft YaHei UI Light", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateSubmitted.Location = New System.Drawing.Point(131, 80)
        Me.lblDateSubmitted.Name = "lblDateSubmitted"
        Me.lblDateSubmitted.Size = New System.Drawing.Size(15, 19)
        Me.lblDateSubmitted.TabIndex = 597
        Me.lblDateSubmitted.Text = "-"
        '
        'btnApprove
        '
        Me.btnApprove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApprove.BackColor = System.Drawing.Color.Navy
        Me.btnApprove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnApprove.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnApprove.Font = New System.Drawing.Font("Microsoft YaHei UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApprove.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnApprove.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnApprove.Location = New System.Drawing.Point(16, 605)
        Me.btnApprove.Name = "btnApprove"
        Me.btnApprove.Size = New System.Drawing.Size(754, 37)
        Me.btnApprove.TabIndex = 598
        Me.btnApprove.Text = "Approve"
        Me.btnApprove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnApprove.UseVisualStyleBackColor = False
        '
        'picProfile
        '
        Me.picProfile.Location = New System.Drawing.Point(12, 107)
        Me.picProfile.Name = "picProfile"
        Me.picProfile.Size = New System.Drawing.Size(229, 216)
        Me.picProfile.TabIndex = 600
        Me.picProfile.TabStop = False
        '
        'lblControlNo
        '
        Me.lblControlNo.AutoSize = True
        Me.lblControlNo.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblControlNo.ForeColor = System.Drawing.Color.DarkRed
        Me.lblControlNo.Location = New System.Drawing.Point(198, 19)
        Me.lblControlNo.Name = "lblControlNo"
        Me.lblControlNo.Size = New System.Drawing.Size(17, 22)
        Me.lblControlNo.TabIndex = 602
        Me.lblControlNo.Text = "-"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Navy
        Me.Label3.Location = New System.Drawing.Point(12, 19)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(180, 22)
        Me.Label3.TabIndex = 604
        Me.Label3.Text = "Appointment Details"
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
        Me.btnClose.Location = New System.Drawing.Point(712, 12)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(53, 39)
        Me.btnClose.TabIndex = 603
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 80)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(117, 19)
        Me.Label1.TabIndex = 612
        Me.Label1.Text = "Date Submitted:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(454, 80)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 19)
        Me.Label5.TabIndex = 617
        Me.Label5.Text = "Status:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(12, 531)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(63, 19)
        Me.Label6.TabIndex = 616
        Me.Label6.Text = "Address"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(12, 489)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(51, 19)
        Me.Label7.TabIndex = 615
        Me.Label7.Text = "Phone"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(12, 443)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(53, 19)
        Me.Label8.TabIndex = 614
        Me.Label8.Text = "Name:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(412, 489)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(45, 19)
        Me.Label9.TabIndex = 613
        Me.Label9.Text = "Email"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 395)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 19)
        Me.Label2.TabIndex = 618
        Me.Label2.Text = "Service Type"
        '
        'btnReject
        '
        Me.btnReject.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnReject.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnReject.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnReject.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReject.Font = New System.Drawing.Font("Microsoft YaHei UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReject.ForeColor = System.Drawing.Color.DarkBlue
        Me.btnReject.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnReject.Location = New System.Drawing.Point(16, 648)
        Me.btnReject.Name = "btnReject"
        Me.btnReject.Size = New System.Drawing.Size(754, 37)
        Me.btnReject.TabIndex = 619
        Me.btnReject.Text = "Reject"
        Me.btnReject.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnReject.UseVisualStyleBackColor = False
        '
        'picIDFront
        '
        Me.picIDFront.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.picIDFront.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.picIDFront.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picIDFront.Location = New System.Drawing.Point(247, 108)
        Me.picIDFront.Name = "picIDFront"
        Me.picIDFront.Size = New System.Drawing.Size(205, 105)
        Me.picIDFront.TabIndex = 620
        Me.picIDFront.TabStop = False
        '
        'picIDBack
        '
        Me.picIDBack.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.picIDBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.picIDBack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picIDBack.Location = New System.Drawing.Point(247, 218)
        Me.picIDBack.Name = "picIDBack"
        Me.picIDBack.Size = New System.Drawing.Size(205, 105)
        Me.picIDBack.TabIndex = 621
        Me.picIDBack.TabStop = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.Label4.Font = New System.Drawing.Font("Microsoft YaHei UI Light", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(252, 219)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(36, 19)
        Me.Label4.TabIndex = 624
        Me.Label4.Text = "Back"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.Label10.Font = New System.Drawing.Font("Microsoft YaHei UI Light", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(251, 109)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(41, 19)
        Me.Label10.TabIndex = 623
        Me.Label10.Text = "Front"
        '
        'RtbAddress
        '
        Me.RtbAddress.BackColor = System.Drawing.SystemColors.Control
        Me.RtbAddress.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RtbAddress.Location = New System.Drawing.Point(81, 531)
        Me.RtbAddress.Name = "RtbAddress"
        Me.RtbAddress.Size = New System.Drawing.Size(685, 42)
        Me.RtbAddress.TabIndex = 625
        Me.RtbAddress.Text = ""
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.Label11.Font = New System.Drawing.Font("Microsoft YaHei UI Light", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(620, 108)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(36, 19)
        Me.Label11.TabIndex = 630
        Me.Label11.Text = "Back"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.Label12.Font = New System.Drawing.Font("Microsoft YaHei UI Light", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(462, 108)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(41, 19)
        Me.Label12.TabIndex = 629
        Me.Label12.Text = "Front"
        '
        'picRepID
        '
        Me.picRepID.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.picRepID.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.picRepID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picRepID.Location = New System.Drawing.Point(615, 107)
        Me.picRepID.Name = "picRepID"
        Me.picRepID.Size = New System.Drawing.Size(151, 216)
        Me.picRepID.TabIndex = 628
        Me.picRepID.TabStop = False
        '
        'picAuthLetter
        '
        Me.picAuthLetter.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.picAuthLetter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.picAuthLetter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picAuthLetter.Location = New System.Drawing.Point(458, 107)
        Me.picAuthLetter.Name = "picAuthLetter"
        Me.picAuthLetter.Size = New System.Drawing.Size(151, 216)
        Me.picAuthLetter.TabIndex = 627
        Me.picAuthLetter.TabStop = False
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(12, 347)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(88, 19)
        Me.Label13.TabIndex = 634
        Me.Label13.Text = "Request For"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(409, 347)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(109, 19)
        Me.Label14.TabIndex = 633
        Me.Label14.Text = "Representative"
        '
        'lblRequestFor
        '
        Me.lblRequestFor.AutoSize = True
        Me.lblRequestFor.Font = New System.Drawing.Font("Microsoft YaHei UI Light", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRequestFor.Location = New System.Drawing.Point(106, 347)
        Me.lblRequestFor.Name = "lblRequestFor"
        Me.lblRequestFor.Size = New System.Drawing.Size(15, 19)
        Me.lblRequestFor.TabIndex = 632
        Me.lblRequestFor.Text = "-"
        '
        'lblRepresentativeName
        '
        Me.lblRepresentativeName.AutoSize = True
        Me.lblRepresentativeName.Font = New System.Drawing.Font("Microsoft YaHei UI Light", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRepresentativeName.Location = New System.Drawing.Point(524, 347)
        Me.lblRepresentativeName.Name = "lblRepresentativeName"
        Me.lblRepresentativeName.Size = New System.Drawing.Size(15, 19)
        Me.lblRepresentativeName.TabIndex = 631
        Me.lblRepresentativeName.Text = "-"
        '
        'frmAppointmentDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(777, 697)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.lblRequestFor)
        Me.Controls.Add(Me.lblRepresentativeName)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.picRepID)
        Me.Controls.Add(Me.picAuthLetter)
        Me.Controls.Add(Me.RtbAddress)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.picIDBack)
        Me.Controls.Add(Me.picIDFront)
        Me.Controls.Add(Me.btnReject)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lblControlNo)
        Me.Controls.Add(Me.picProfile)
        Me.Controls.Add(Me.btnApprove)
        Me.Controls.Add(Me.lblDateSubmitted)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.lblPhone)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.lblEmail)
        Me.Controls.Add(Me.cboServiceType)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmAppointmentDetails"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmAppointmentDetails"
        CType(Me.picProfile, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picIDFront, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picIDBack, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picRepID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picAuthLetter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblEmail As Label
    Friend WithEvents cboServiceType As ComboBox
    Friend WithEvents lblName As Label
    Friend WithEvents lblPhone As Label
    Friend WithEvents lblStatus As Label
    Friend WithEvents lblDateSubmitted As Label
    Friend WithEvents btnApprove As Button
    Friend WithEvents picProfile As PictureBox
    Friend WithEvents lblControlNo As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents btnClose As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents btnReject As Button
    Friend WithEvents picIDFront As PictureBox
    Friend WithEvents picIDBack As PictureBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents RtbAddress As RichTextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents picRepID As PictureBox
    Friend WithEvents picAuthLetter As PictureBox
    Friend WithEvents Label13 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents lblRequestFor As Label
    Friend WithEvents lblRepresentativeName As Label
End Class
