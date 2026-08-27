Imports MySql.Data.MySqlClient
Imports System.IO

Public Class frmUser_Dashboard

    Private loggedResidentID As Integer = 0
    Private activeFilterLabel As Label = Nothing

    Private Sub frmUser_Dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblDateTime.Text = DateTime.Now.ToString("F")
        Timer1.Interval = 1000
        Timer1.Start()

        ' Apply custom grid styling
        StyleDataGridView(dgvRequests)

        SetActiveLabel(lblPending)
        RefreshDashboardData()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lblDateTime.Text = DateTime.Now.ToString("F")
    End Sub

    ' --- GENERAL REFRESH SUBROUTINE ---
    Private Sub RefreshDashboardData()
        LoadResidentProfile()
        LoadStatusCounts()

        ' Maintain current selected filter when refreshing
        Dim currentStatus As String = "PENDING"
        If activeFilterLabel IsNot Nothing Then
            If activeFilterLabel Is lblApproved Then
                currentStatus = "APPROVED"
            ElseIf activeFilterLabel Is lblRejected Then
                currentStatus = "REJECTED"
            ElseIf activeFilterLabel Is lblCompleted Then
                currentStatus = "COMPLETED"
            ElseIf activeFilterLabel Is lblCancelled Then
                currentStatus = "CANCELLED"
            End If
        End If

        LoadUserRequests(currentStatus)
    End Sub

    ' --- REFRESH BUTTON CLICK HANDLER ---
    Private Sub btnRef_Click(sender As Object, e As EventArgs) Handles btnRef.Click
        RefreshDashboardData()
        MsgBox("Dashboard refreshed successfully!", MsgBoxStyle.Information, "Refreshed")
    End Sub

    ' --- CUSTOM DATAGRIDVIEW DESIGN ---
    Private Sub StyleDataGridView(dgv As DataGridView)
        dgv.EnableHeadersVisualStyles = False
        dgv.BorderStyle = BorderStyle.None
        dgv.BackgroundColor = Color.White
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        dgv.GridColor = Color.FromArgb(220, 224, 230)
        dgv.RowHeadersVisible = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.MultiSelect = False
        dgv.AllowUserToResizeRows = False

        ' Header Styling
        Dim headerStyle As New DataGridViewCellStyle()
        headerStyle.BackColor = Color.FromArgb(248, 249, 252)
        headerStyle.ForeColor = Color.FromArgb(50, 50, 60)
        headerStyle.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
        headerStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        headerStyle.Padding = New Padding(10, 8, 10, 8)

        dgv.ColumnHeadersDefaultCellStyle = headerStyle
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        dgv.ColumnHeadersHeight = 40
        dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing

        ' Default Row Styling
        Dim defaultRowStyle As New DataGridViewCellStyle()
        defaultRowStyle.BackColor = Color.White
        defaultRowStyle.ForeColor = Color.FromArgb(50, 50, 60)
        defaultRowStyle.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular)
        defaultRowStyle.SelectionBackColor = Color.FromArgb(210, 215, 240)
        defaultRowStyle.SelectionForeColor = Color.Black
        defaultRowStyle.Padding = New Padding(10, 4, 10, 4)

        ' Alternating Row Styling
        Dim alternatingRowStyle As New DataGridViewCellStyle(defaultRowStyle)
        alternatingRowStyle.BackColor = Color.FromArgb(235, 237, 255)

        dgv.DefaultCellStyle = defaultRowStyle
        dgv.AlternatingRowsDefaultCellStyle = alternatingRowStyle
        dgv.RowTemplate.Height = 32
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    ' --- 1. LOAD PROFILE DATA & PICTURE FROM DATABASE ---
    Private Sub LoadResidentProfile()
        Try
            connection()

            sql = "SELECT ResidentID, FullName, Email, MobileNumber, Birthday, BirthPlace, Gender, CivilStatus, Picture " &
                  "FROM residences WHERE FullName=@name OR Username=@name"

            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@name", LoggedFullname)
            dr = cmd.ExecuteReader()

            Dim profileFound As Boolean = False

            If dr.Read() Then
                profileFound = True
                loggedResidentID = If(IsDBNull(dr("ResidentID")), 0, Convert.ToInt32(dr("ResidentID")))
                lblFullName.Text = dr("FullName").ToString()
                lblEmailInfo.Text = If(IsDBNull(dr("Email")), "-", dr("Email").ToString())
                lblContactNumber.Text = If(IsDBNull(dr("MobileNumber")), "-", dr("MobileNumber").ToString())

                If Not IsDBNull(dr("Birthday")) Then
                    lblBirthday.Text = Convert.ToDateTime(dr("Birthday")).ToString("MMM dd, yyyy")
                Else
                    lblBirthday.Text = "-"
                End If

                lblBirthplace.Text = If(IsDBNull(dr("BirthPlace")), "-", dr("BirthPlace").ToString())
                lblGender.Text = If(IsDBNull(dr("Gender")), "-", dr("Gender").ToString())
                lblCivilStatus.Text = If(IsDBNull(dr("CivilStatus")), "-", dr("CivilStatus").ToString())

                If Not IsDBNull(dr("Picture")) Then
                    Dim imgBytes As Byte() = CType(dr("Picture"), Byte())
                    Using ms As New MemoryStream(imgBytes)
                        picUserProfile.SizeMode = PictureBoxSizeMode.StretchImage
                        If picUserProfile.Image IsNot Nothing Then picUserProfile.Image.Dispose()
                        picUserProfile.Image = Image.FromStream(ms)
                    End Using
                End If
            End If
            dr.Close()

            If Not profileFound Then
                sql = "SELECT UserID, FullName, Email, MobileNumber, Birthday, BirthPlace, Gender, CivilStatus, Picture " &
                      "FROM users WHERE FullName=@name OR Username=@name"

                cmd = New MySqlCommand(sql, cn)
                cmd.Parameters.AddWithValue("@name", LoggedFullname)
                dr = cmd.ExecuteReader()

                If dr.Read() Then
                    loggedResidentID = If(IsDBNull(dr("UserID")), 0, Convert.ToInt32(dr("UserID")))
                    lblFullName.Text = dr("FullName").ToString()
                    lblEmailInfo.Text = If(IsDBNull(dr("Email")), "-", dr("Email").ToString())
                    lblContactNumber.Text = If(IsDBNull(dr("MobileNumber")), "-", dr("MobileNumber").ToString())

                    If Not IsDBNull(dr("Birthday")) Then
                        lblBirthday.Text = Convert.ToDateTime(dr("Birthday")).ToString("MMM dd, yyyy")
                    Else
                        lblBirthday.Text = "-"
                    End If

                    lblBirthplace.Text = If(IsDBNull(dr("BirthPlace")), "-", dr("BirthPlace").ToString())
                    lblGender.Text = If(IsDBNull(dr("Gender")), "-", dr("Gender").ToString())
                    lblCivilStatus.Text = If(IsDBNull(dr("CivilStatus")), "-", dr("CivilStatus").ToString())

                    If Not IsDBNull(dr("Picture")) Then
                        Dim imgBytes As Byte() = CType(dr("Picture"), Byte())
                        Using ms As New MemoryStream(imgBytes)
                            picUserProfile.SizeMode = PictureBoxSizeMode.StretchImage
                            If picUserProfile.Image IsNot Nothing Then picUserProfile.Image.Dispose()
                            picUserProfile.Image = Image.FromStream(ms)
                        End Using
                    End If
                End If
                dr.Close()
            End If

            lblWelcomeUser.Text = $"{lblFullName.Text}!"

        Catch ex As Exception
            MsgBox("Error loading profile: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            CloseConnection()
        End Try
    End Sub

    ' --- 2. LOAD STATUS COUNTS FOR LABELS ---
    Private Sub LoadStatusCounts()
        Dim pendingCount As Integer = 0
        Dim approvedCount As Integer = 0
        Dim rejectedCount As Integer = 0
        Dim completedCount As Integer = 0
        Dim cancelledCount As Integer = 0

        Try
            connection()

            sql = "SELECT UPPER(Status) AS StatusName, COUNT(*) AS Total " &
                  "FROM appointments " &
                  "WHERE (FullName = @fullname OR ResidentID = @resID) " &
                  "GROUP BY UPPER(Status)"

            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@fullname", LoggedFullname)
            cmd.Parameters.AddWithValue("@resID", loggedResidentID)

            dr = cmd.ExecuteReader()
            While dr.Read()
                Dim st As String = dr("StatusName").ToString().Trim()
                Dim count As Integer = Convert.ToInt32(dr("Total"))

                Select Case st
                    Case "PENDING"
                        pendingCount = count
                    Case "APPROVED"
                        approvedCount = count
                    Case "REJECTED"
                        rejectedCount = count
                    Case "COMPLETED"
                        completedCount = count
                    Case "CANCELLED"
                        cancelledCount = count
                End Select
            End While
            dr.Close()

            ' Update label captions with counts
            lblPending.Text = $"Pending ({pendingCount})"
            lblApproved.Text = $"Approved ({approvedCount})"
            lblRejected.Text = $"Rejected ({rejectedCount})"
            lblCompleted.Text = $"Completed ({completedCount})"
            lblCancelled.Text = $"Cancelled ({cancelledCount})"

        Catch ex As Exception
            ' Keep default captions if count query fails
        Finally
            CloseConnection()
        End Try
    End Sub

    ' --- 3. LOAD REQUESTS INTO DATAGRIDVIEW ---
    Private Sub LoadUserRequests(Optional statusFilter As String = "")
        Try
            connection()

            sql = "SELECT ControlNo AS 'Control No.', RequestType AS 'Request Type', Purpose, " &
                  "Department, DateSubmitted AS 'Date Submitted', Status " &
                  "FROM appointments " &
                  "WHERE (FullName = @fullname OR ResidentID = @resID) "

            If Not String.IsNullOrEmpty(statusFilter) Then
                sql &= "AND UPPER(Status) = @status "
            End If

            sql &= "ORDER BY AppointmentID DESC"

            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@fullname", LoggedFullname)
            cmd.Parameters.AddWithValue("@resID", loggedResidentID)

            If Not String.IsNullOrEmpty(statusFilter) Then
                cmd.Parameters.AddWithValue("@status", statusFilter.ToUpper())
            End If

            Dim da As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            dgvRequests.DataSource = dt

            ' Tint the first column (Control No.)
            If dgvRequests.Columns.Count > 0 Then
                dgvRequests.Columns(0).DefaultCellStyle.BackColor = Color.FromArgb(220, 225, 255)
            End If

        Catch ex As Exception
            MsgBox("Error loading requests: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            CloseConnection()
        End Try
    End Sub

    ' --- 4. STATUS FILTER LABELS ---
    Private Sub lblPending_Click(sender As Object, e As EventArgs) Handles lblPending.Click
        SetActiveLabel(lblPending)
        LoadUserRequests("PENDING")
    End Sub

    Private Sub lblApproved_Click(sender As Object, e As EventArgs) Handles lblApproved.Click
        SetActiveLabel(lblApproved)
        LoadUserRequests("APPROVED")
    End Sub

    Private Sub lblRejected_Click(sender As Object, e As EventArgs) Handles lblRejected.Click
        SetActiveLabel(lblRejected)
        LoadUserRequests("REJECTED")
    End Sub

    Private Sub lblCompleted_Click(sender As Object, e As EventArgs) Handles lblCompleted.Click
        SetActiveLabel(lblCompleted)
        LoadUserRequests("COMPLETED")
    End Sub

    Private Sub lblCancelled_Click(sender As Object, e As EventArgs) Handles lblCancelled.Click
        SetActiveLabel(lblCancelled)
        LoadUserRequests("CANCELLED")
    End Sub

    Private Sub SetActiveLabel(targetLabel As Label)
        Dim allFilterLabels As Label() = {lblPending, lblApproved, lblRejected, lblCompleted, lblCancelled}

        For Each lbl As Label In allFilterLabels
            If lbl IsNot Nothing Then
                lbl.Font = New Font(lbl.Font, FontStyle.Regular)
                lbl.ForeColor = Color.Black
            End If
        Next

        If targetLabel IsNot Nothing Then
            targetLabel.Font = New Font(targetLabel.Font, FontStyle.Bold)
            targetLabel.ForeColor = Color.Navy
            activeFilterLabel = targetLabel
        End If
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Dim filterText As String = txtSearch.Text.Trim().Replace("'", "''")
        If dgvRequests.DataSource IsNot Nothing Then
            Dim dt As DataTable = CType(dgvRequests.DataSource, DataTable)
            dt.DefaultView.RowFilter = $"`Control No.` LIKE '%{filterText}%' OR `Request Type` LIKE '%{filterText}%' OR Purpose LIKE '%{filterText}%'"
        End If
    End Sub

    Private Sub btnCreateRequest_Click(sender As Object, e As EventArgs) Handles btnCreateRequest.Click
        Dim frm As New frmCreateAppointment()
        If frm.ShowDialog() = DialogResult.OK Then
            RefreshDashboardData()
        End If
    End Sub
End Class