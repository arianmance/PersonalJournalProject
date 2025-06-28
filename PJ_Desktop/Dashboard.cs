using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PJ_Desktop
{
    public partial class Dashboard : Form
    {
        private readonly string username;
        private readonly string connectionString = "Data Source=arianmance\\SQLEXPRESS;Initial Catalog=JournalDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

        public Dashboard(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            lblGreet.Text = $"Welcome, {username}!";
            LoadEntries();
        }

        private void LoadEntries()
        {
            JournalEntry.Items.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Content FROM JournalEntries WHERE Username = @Username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    JournalEntry.Items.Add(reader["Content"].ToString());
                }
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            string content = Microsoft.VisualBasic.Interaction.InputBox("Enter your journal entry:", "Create Entry");
            if (string.IsNullOrWhiteSpace(content))
            {
                MessageBox.Show("Entry cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO JournalEntries (Username, Content) VALUES (@Username, @Content)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Content", content);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Entry added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadEntries();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            int index = JournalEntry.SelectedIndex;
            if (index >= 0)
            {
                string selectedEntry = JournalEntry.SelectedItem.ToString();
                MessageBox.Show(selectedEntry, "View Entry");
            }
            else
            {
                MessageBox.Show("Please select an entry to view.");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int index = JournalEntry.SelectedIndex;
            if (index < 0)
            {
                MessageBox.Show("Please select an entry to update.");
                return;
            }

            string oldContent = JournalEntry.SelectedItem.ToString();
            string newContent = Microsoft.VisualBasic.Interaction.InputBox("Edit your entry:", "Update Entry", oldContent);

            if (string.IsNullOrWhiteSpace(newContent))
            {
                MessageBox.Show("Updated content cannot be empty.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE JournalEntries SET Content = @NewContent WHERE Username = @Username AND Content = @OldContent";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NewContent", newContent);
                cmd.Parameters.AddWithValue("@OldContent", oldContent);
                cmd.Parameters.AddWithValue("@Username", username);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Entry updated successfully!", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadEntries();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int index = JournalEntry.SelectedIndex;
            if (index < 0)
            {
                MessageBox.Show("Please select an entry to delete.");
                return;
            }

            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this entry?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            string content = JournalEntry.SelectedItem.ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM JournalEntries WHERE Username = @Username AND Content = @Content";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Content", content);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Entry deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadEntries();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadEntries();
                return;
            }

            JournalEntry.Items.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Content FROM JournalEntries WHERE Username = @Username AND Content LIKE @Keyword";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    JournalEntry.Items.Add(reader["Content"].ToString());
                }
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginForm login = new LoginForm();
            login.Show();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void JournalEntry_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}