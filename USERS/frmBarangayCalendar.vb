Imports MySql.Data.MySqlClient
Imports System.Drawing.Drawing2D

Public Class frmBarangayCalendar

    Private currentDisplayDate As DateTime = DateTime.Today

    Private Sub frmBarangayCalendar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 1. Start live date and time timer
        lblDateTime.Text = DateTime.Now.ToString("F")
        Timer1.Interval = 1000
        Timer1.Start()

        ' 2. Load user welcome name
        LoadUserProfile()

        ' 3. Configure FlowLayoutPanel Grid
        flpCalendarGrid.WrapContents = True
        flpCalendarGrid.AutoScroll = True

        ' 4. Render initial calendar month
        DisplayMonthCalendar()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lblDateTime.Text = DateTime.Now.ToString("F")
    End Sub

    ' --- 1. LOAD USER PROFILE NAME ---
    Private Sub LoadUserProfile()
        Try
            connection()
            sql = "SELECT FullName FROM residences WHERE FullName=@name OR Username=@name"
            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@name", LoggedFullname)
            dr = cmd.ExecuteReader()

            If dr.Read() Then
                lblWelcomeUser.Text = dr("FullName").ToString() & "!"
            Else
                lblWelcomeUser.Text = LoggedFullname & "!"
            End If
            dr.Close()

        Catch ex As Exception
            lblWelcomeUser.Text = LoggedFullname & "!"
        Finally
            CloseConnection()
        End Try
    End Sub

    ' --- 2. MONTH NAVIGATION ---
    Private Sub btnPrevMonth_Click(sender As Object, e As EventArgs) Handles btnPrevMonth.Click
        currentDisplayDate = currentDisplayDate.AddMonths(-1)
        DisplayMonthCalendar()
    End Sub

    Private Sub btnNextMonth_Click(sender As Object, e As EventArgs) Handles btnNextMonth.Click
        currentDisplayDate = currentDisplayDate.AddMonths(1)
        DisplayMonthCalendar()
    End Sub

    ' --- 3. RENDER MONTH GRID CELLS ---
    Private Sub DisplayMonthCalendar()
        flpCalendarGrid.Controls.Clear()

        If lblCurrentMonthYear IsNot Nothing Then
            lblCurrentMonthYear.Text = currentDisplayDate.ToString("MMMM yyyy")
        End If

        Dim firstDayOfMonth As New DateTime(currentDisplayDate.Year, currentDisplayDate.Month, 1)
        Dim daysInMonth As Integer = DateTime.DaysInMonth(currentDisplayDate.Year, currentDisplayDate.Month)
        Dim dayOfWeekOffset As Integer = CInt(firstDayOfMonth.DayOfWeek)

        Dim cellWidth As Integer = (flpCalendarGrid.Width - 40) \ 7
        Dim cellHeight As Integer = 95

        ' Blank padding panels for offset
        For i As Integer = 0 To dayOfWeekOffset - 1
            Dim blankPanel As New Panel With {
                .Size = New Size(cellWidth, cellHeight),
                .BackColor = Color.FromArgb(245, 247, 250),
                .Margin = New Padding(1)
            }
            flpCalendarGrid.Controls.Add(blankPanel)
        Next

        ' Fetch appointment counts
        Dim appointmentCounts As Dictionary(Of Integer, Integer) = GetMonthlyAppointmentCounts(currentDisplayDate.Year, currentDisplayDate.Month)

        ' Generate Day Cells
        For day As Integer = 1 To daysInMonth
            Dim dateOfCell As New DateTime(currentDisplayDate.Year, currentDisplayDate.Month, day)
            Dim isWeekend As Boolean = (dateOfCell.DayOfWeek = DayOfWeek.Saturday OrElse dateOfCell.DayOfWeek = DayOfWeek.Sunday)
            Dim count As Integer = If(appointmentCounts.ContainsKey(day), appointmentCounts(day), 0)

            Dim pnlDay As New Panel With {
                .Size = New Size(cellWidth, cellHeight),
                .BorderStyle = BorderStyle.FixedSingle,
                .Margin = New Padding(1),
                .Tag = dateOfCell,
                .Cursor = Cursors.Hand
            }

            Dim lblDayNum As New Label With {
                .Text = day.ToString(),
                .Font = New Font("Segoe UI", 9.5F, FontStyle.Bold),
                .Dock = DockStyle.Top,
                .Height = 22,
                .Padding = New Padding(4, 2, 0, 0)
            }

            Dim lblCount As New Label With {
                .Text = $"Scheduled: {count}",
                .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold),
                .Dock = DockStyle.Fill,
                .TextAlign = ContentAlignment.MiddleCenter
            }

            Dim lblStatus As New Label With {
                .Dock = DockStyle.Bottom,
                .Height = 20,
                .Font = New Font("Segoe UI", 7.5F, FontStyle.Italic),
                .TextAlign = ContentAlignment.MiddleCenter
            }

            If isWeekend Then
                pnlDay.BackColor = Color.FromArgb(240, 240, 245)
                lblDayNum.ForeColor = Color.Gray
                lblCount.ForeColor = Color.Gray
                lblStatus.Text = "CLOSED"
                lblStatus.ForeColor = Color.DarkRed
            Else
                lblStatus.Text = "8:00 AM - 5:00 PM"
                lblStatus.ForeColor = Color.FromArgb(40, 40, 50)

                If count >= 20 Then
                    pnlDay.BackColor = Color.FromArgb(255, 200, 200)
                    lblCount.ForeColor = Color.DarkRed
                ElseIf count >= 10 Then
                    pnlDay.BackColor = Color.FromArgb(255, 245, 180)
                    lblCount.ForeColor = Color.FromArgb(140, 90, 0)
                Else
                    pnlDay.BackColor = Color.FromArgb(210, 245, 210)
                    lblCount.ForeColor = Color.DarkGreen
                End If

                If dateOfCell.Date = DateTime.Today Then
                    lblDayNum.ForeColor = Color.Navy
                    lblDayNum.Text &= " (Today)"
                End If
            End If

            pnlDay.Controls.Add(lblCount)
            pnlDay.Controls.Add(lblDayNum)
            pnlDay.Controls.Add(lblStatus)

            ' Click Event to View Daily List
            AddHandler pnlDay.Click, Sub(s, ev) OnDayCellClicked(dateOfCell, isWeekend)
            AddHandler lblDayNum.Click, Sub(s, ev) OnDayCellClicked(dateOfCell, isWeekend)
            AddHandler lblCount.Click, Sub(s, ev) OnDayCellClicked(dateOfCell, isWeekend)

            flpCalendarGrid.Controls.Add(pnlDay)
        Next
    End Sub

    ' --- 4. FETCH MONTHLY COUNTS ---
    Private Function GetMonthlyAppointmentCounts(year As Integer, month As Integer) As Dictionary(Of Integer, Integer)
        Dim countMap As New Dictionary(Of Integer, Integer)()

        Try
            connection()
            sql = "SELECT DAY(ScheduledDate) AS SchedDay, COUNT(*) AS TotalAppts " &
                  "FROM appointments " &
                  "WHERE YEAR(ScheduledDate) = @yr AND MONTH(ScheduledDate) = @mo AND UPPER(Status) != 'CANCELLED' " &
                  "GROUP BY DAY(ScheduledDate)"

            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@yr", year)
            cmd.Parameters.AddWithValue("@mo", month)
            dr = cmd.ExecuteReader()

            While dr.Read()
                If Not IsDBNull(dr("SchedDay")) Then
                    Dim dayNum As Integer = Convert.ToInt32(dr("SchedDay"))
                    Dim total As Integer = Convert.ToInt32(dr("TotalAppts"))
                    countMap(dayNum) = total
                End If
            End While
            dr.Close()

        Catch ex As Exception
            ' Keep empty on error
        Finally
            CloseConnection()
        End Try

        Return countMap
    End Function

    ' --- 5. OPEN DAILY APPOINTMENTS LISTVIEW FORM ON DAY CLICK ---
    Private Sub OnDayCellClicked(selectedDate As DateTime, isWeekend As Boolean)
        If isWeekend Then
            MsgBox("The Barangay Office is closed on weekends.", MsgBoxStyle.Exclamation, "Office Closed")
            Return
        End If

        ' Opens a ListView modal displaying all pick-ups for the selected day
        Using frmList As New frmDayAppointmentsList(selectedDate)
            frmList.ShowDialog()
            ' Refresh calendar counts after returning
            DisplayMonthCalendar()
        End Using
    End Sub

End Class