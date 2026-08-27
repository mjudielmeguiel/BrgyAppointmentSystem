Imports MySql.Data.MySqlClient

Public Class frmCreateAppointment

    ' Selected resident details
    Private selectedResidentID As Integer = 0
    Private selectedFullName As String = ""
    Private selectedEmail As String = ""
    Private selectedPhone As String = ""
    Private selectedAddress As String = ""

    Private Sub frmCreateAppointment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Setup Request For Options
        cboRequestFor.Items.Clear()
        cboRequestFor.Items.AddRange({"Myself", "Relative / Other"})
        cboRequestFor.SelectedIndex = 0

        ' Setup Request Type Options
        cboRequestType.Items.Clear()
        cboRequestType.Items.AddRange({
            "Barangay Clearance",
            "Certificate of Indigency",
            "Barangay ID",
            "Business Permit",
            "First Time Job Seeker Certificate",
            "Certificate of Residency"
        })
        cboRequestType.SelectedIndex = 0

        ' Ensure Browse Button stays enabled
        btnBrowseName.Enabled = True

        ' Load current logged user by default
        LoadLoggedUserData()
    End Sub

    ' --- LOAD LOGGED USER DETAILS ---
    Private Sub LoadLoggedUserData()
        Try
            connection()
            ' First search in residences table
            sql = "SELECT ResidentID, FullName, Email, MobileNumber, Address FROM residences WHERE FullName=@name OR Username=@name"
            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@name", LoggedFullname)
            dr = cmd.ExecuteReader()

            If dr.Read() Then
                selectedResidentID = If(IsDBNull(dr("ResidentID")), 0, Convert.ToInt32(dr("ResidentID")))
                selectedFullName = dr("FullName").ToString()
                selectedEmail = If(IsDBNull(dr("Email")), "", dr("Email").ToString())
                selectedPhone = If(IsDBNull(dr("MobileNumber")), "", dr("MobileNumber").ToString())
                selectedAddress = If(IsDBNull(dr("Address")), "", dr("Address").ToString())

                txtName.Text = selectedFullName
                txtName.ReadOnly = True
            Else
                dr.Close()
                ' Fallback search in users table (Staff / System Admin)
                sql = "SELECT UserID, FullName, Email, MobileNumber, Address FROM users WHERE FullName=@name OR Username=@name"
                cmd = New MySqlCommand(sql, cn)
                cmd.Parameters.AddWithValue("@name", LoggedFullname)
                dr = cmd.ExecuteReader()

                If dr.Read() Then
                    ' User is a staff/admin, not in residences table
                    selectedResidentID = 0
                    selectedFullName = dr("FullName").ToString()
                    selectedEmail = If(IsDBNull(dr("Email")), "", dr("Email").ToString())
                    selectedPhone = If(IsDBNull(dr("MobileNumber")), "", dr("MobileNumber").ToString())
                    selectedAddress = If(IsDBNull(dr("Address")), "", dr("Address").ToString())

                    txtName.Text = selectedFullName
                    txtName.ReadOnly = True
                End If
            End If
            dr.Close()
        Catch ex As Exception
            MsgBox("Error loading user data: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            CloseConnection()
        End Try
    End Sub

    ' --- DROPDOWN SELECTION CHANGE ---
    Private Sub cboRequestFor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRequestFor.SelectedIndexChanged
        If cboRequestFor.Text = "Myself" Then
            LoadLoggedUserData()
        Else
            txtName.Clear()
            txtName.ReadOnly = False
            selectedResidentID = 0
            selectedFullName = ""
            selectedEmail = ""
            selectedPhone = ""
            selectedAddress = ""
        End If
    End Sub

    ' --- BROWSE BUTTON CLICK HANDLER (ALWAYS ENABLED) ---
    Private Sub btnBrowseName_Click(sender As Object, e As EventArgs) Handles btnBrowseName.Click
        Using frmList As New ResidenceList()
            If frmList.ShowDialog() = DialogResult.OK Then
                ' Retrieve selected resident details from ResidenceList
                selectedResidentID = frmList.SelectedResidentID
                selectedFullName = frmList.SelectedFullName
                selectedEmail = frmList.SelectedEmail
                selectedPhone = frmList.SelectedPhone
                selectedAddress = frmList.SelectedAddress

                txtName.Text = selectedFullName
                txtName.ReadOnly = True
            End If
        End Using
    End Sub

    ' --- GENERATE CONTROL NUMBER (APP-001) ---
    Private Function GenerateControlNo() As String
        Dim newControlNo As String = "APP-001"
        Try
            connection()
            sql = "SELECT ControlNo FROM appointments WHERE ControlNo LIKE 'APP-%' ORDER BY AppointmentID DESC LIMIT 1"
            cmd = New MySqlCommand(sql, cn)
            dr = cmd.ExecuteReader()

            If dr.Read() Then
                Dim lastCode As String = dr("ControlNo").ToString()
                If lastCode.Contains("-") Then
                    Dim parts() As String = lastCode.Split("-"c)
                    Dim numericPart As Integer
                    If Integer.TryParse(parts(1), numericPart) Then
                        newControlNo = "APP-" & (numericPart + 1).ToString("D3")
                    End If
                End If
            End If
            dr.Close()
        Catch ex As Exception
        Finally
            CloseConnection()
        End Try
        Return newControlNo
    End Function

    ' --- VERIFY IF RESIDENT ID EXISTS IN RESIDENCES TABLE ---
    Private Function VerifyResidentExists(resID As Integer) As Boolean
        If resID <= 0 Then Return False
        Dim exists As Boolean = False
        Try
            connection()
            ' Dynamic check for either ResidentID or id primary key column
            Dim checkSql As String = "SELECT COUNT(*) FROM residences WHERE ResidentID = @id"
            Using checkCmd As New MySqlCommand(checkSql, cn)
                checkCmd.Parameters.AddWithValue("@id", resID)
                Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                exists = (count > 0)
            End Using
        Catch ex As Exception
            ' Try fallback primary key column 'id' if 'ResidentID' column name differs
            Try
                Dim checkSql2 As String = "SELECT COUNT(*) FROM residences WHERE id = @id"
                Using checkCmd2 As New MySqlCommand(checkSql2, cn)
                    checkCmd2.Parameters.AddWithValue("@id", resID)
                    Dim count As Integer = Convert.ToInt32(checkCmd2.ExecuteScalar())
                    exists = (count > 0)
                End Using
            Catch
                exists = False
            End Try
        Finally
            CloseConnection()
        End Try
        Return exists
    End Function

    ' --- SUBMIT APPOINTMENT REQUEST ---
    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If String.IsNullOrWhiteSpace(txtName.Text) Then
            MsgBox("Please enter or select a Name for the request.", MsgBoxStyle.Exclamation, "Validation Error")
            Return
        End If

        If cboRequestType.SelectedIndex = -1 Then
            MsgBox("Please select a Request Type.", MsgBoxStyle.Exclamation, "Validation Error")
            Return
        End If

        ' Verify if selectedResidentID exists in residences table to prevent FK errors
        Dim validResidentID As Object = DBNull.Value
        If selectedResidentID > 0 AndAlso VerifyResidentExists(selectedResidentID) Then
            validResidentID = selectedResidentID
        End If

        Dim controlNumber As String = GenerateControlNo()
        Dim requestTypeVal As String = cboRequestType.Text.Trim()
        Dim purposeVal As String = $"Request for {requestTypeVal} ({cboRequestFor.Text})"
        Dim departmentVal As String = "GENERAL SERVICES"

        Try
            connection()

            sql = "INSERT INTO appointments " &
                  "(ControlNo, RequestType, ResidentID, FullName, EmailAddress, PhoneNumber, FullAddress, Purpose, Department, DateSubmitted, Status) " &
                  "VALUES (@ctrl, @reqType, @resID, @fname, @email, @phone, @address, @purpose, @dept, NOW(), 'PENDING')"

            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@ctrl", controlNumber)
            cmd.Parameters.AddWithValue("@reqType", requestTypeVal)
            cmd.Parameters.AddWithValue("@resID", validResidentID)
            cmd.Parameters.AddWithValue("@fname", txtName.Text.Trim())
            cmd.Parameters.AddWithValue("@email", selectedEmail)
            cmd.Parameters.AddWithValue("@phone", selectedPhone)
            cmd.Parameters.AddWithValue("@address", selectedAddress)
            cmd.Parameters.AddWithValue("@purpose", purposeVal)
            cmd.Parameters.AddWithValue("@dept", departmentVal)

            cmd.ExecuteNonQuery()

            MsgBox($"Request submitted successfully!{vbCrLf}Control No: {controlNumber}", MsgBoxStyle.Information, "Success")

            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MsgBox("Error submitting request: " & ex.Message, MsgBoxStyle.Critical, "Database Error")
        Finally
            CloseConnection()
        End Try
    End Sub

    ' --- CLOSE FORM HANDLER ---
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class