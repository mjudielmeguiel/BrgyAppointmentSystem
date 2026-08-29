<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDayAppointmentsList
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
        Me.lvwAppointments = New System.Windows.Forms.ListView()
        Me.btnNewAppointment = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.lblTitleDate = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lvwAppointments
        '
        Me.lvwAppointments.HideSelection = False
        Me.lvwAppointments.Location = New System.Drawing.Point(12, 12)
        Me.lvwAppointments.Name = "lvwAppointments"
        Me.lvwAppointments.Size = New System.Drawing.Size(776, 383)
        Me.lvwAppointments.TabIndex = 0
        Me.lvwAppointments.UseCompatibleStateImageBehavior = False
        '
        'btnNewAppointment
        '
        Me.btnNewAppointment.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNewAppointment.BackColor = System.Drawing.Color.Navy
        Me.btnNewAppointment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnNewAppointment.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNewAppointment.Font = New System.Drawing.Font("Microsoft YaHei UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNewAppointment.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnNewAppointment.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnNewAppointment.Location = New System.Drawing.Point(502, 401)
        Me.btnNewAppointment.Name = "btnNewAppointment"
        Me.btnNewAppointment.Size = New System.Drawing.Size(131, 37)
        Me.btnNewAppointment.TabIndex = 507
        Me.btnNewAppointment.Text = "Create Request"
        Me.btnNewAppointment.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnNewAppointment.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft YaHei UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.DarkBlue
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.Location = New System.Drawing.Point(657, 401)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(131, 37)
        Me.btnClose.TabIndex = 553
        Me.btnClose.Text = "Close"
        Me.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'lblTitleDate
        '
        Me.lblTitleDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTitleDate.AutoSize = True
        Me.lblTitleDate.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitleDate.ForeColor = System.Drawing.Color.Navy
        Me.lblTitleDate.Location = New System.Drawing.Point(8, 406)
        Me.lblTitleDate.Name = "lblTitleDate"
        Me.lblTitleDate.Size = New System.Drawing.Size(128, 22)
        Me.lblTitleDate.TabIndex = 554
        Me.lblTitleDate.Text = "Date and Time"
        '
        'frmDayAppointmentsList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.lblTitleDate)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnNewAppointment)
        Me.Controls.Add(Me.lvwAppointments)
        Me.Name = "frmDayAppointmentsList"
        Me.Text = "frmDayAppointmentsList"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lvwAppointments As ListView
    Friend WithEvents btnNewAppointment As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents lblTitleDate As Label
End Class
