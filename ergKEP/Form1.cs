using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ergKEP
{

    public partial class Form1 : Form
    {
        String connectionString = "Data source=Informations.db;Version=3";
        SQLiteConnection connection;
        public Form1()
        {
            InitializeComponent();
        }
        Dictionary<string, string> fieldMappings = new Dictionary<string, string>()
{
    { "Name", "onoma" },
    { "Email", "email" },
    { "Phone", "phone" },
    { "Date of Birth", "birth" },
    { "Type", "type" },
    { "Address", "address" },
    { "Date of Submission", "date" }
};

        private void Form1_Load(object sender, EventArgs e)
        {
            connection = new SQLiteConnection(connectionString);
            comboBoxField.Items.Add("Name");
            comboBoxField.Items.Add("Email");
            comboBoxField.Items.Add("Phone");
            comboBoxField.Items.Add("Date of Birth");
            comboBoxField.Items.Add("Type");
            comboBoxField.Items.Add("Address");
            comboBoxField.Items.Add("Date of Submission");
            comboBoxField.SelectedIndex = 0;
            comboBox1.Items.Add("Name");
            comboBox1.Items.Add("Email");
            comboBox1.Items.Add("Phone");
            comboBox1.Items.Add("Date of Birth");
            comboBox1.Items.Add("Type");
            comboBox1.Items.Add("Address");
            comboBox1.Items.Add("Date of Submission");
            comboBox1.SelectedIndex = 0;
            HelpButton helpButton = new HelpButton();
            button8.Click += new EventHandler(helpButton.ShowHelpMessage);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            String insert = "Insert into Inf values(@onoma,@email,@phone,@birth,@type,@address,@date)";
            SQLiteCommand command = new SQLiteCommand(insert, connection);
            command.Parameters.AddWithValue("onoma", textBox1.Text);
            command.Parameters.AddWithValue("email", textBox2.Text);
            command.Parameters.AddWithValue("phone", int.Parse(textBox3.Text));
            command.Parameters.AddWithValue("birth", textBox4.Text);
            command.Parameters.AddWithValue("type", textBox5.Text);
            command.Parameters.AddWithValue("address", textBox6.Text);
            command.Parameters.AddWithValue("date", textBox7.Text);
            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("success");
            }
            else
            {
                MessageBox.Show("failed");
            }
            connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                string query = "SELECT * FROM inf";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        StringBuilder results = new StringBuilder();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                results.AppendLine($"Onoma: {reader["onoma"]}, Email: {reader["email"]}, Phone: {reader["phone"]}, " +
                                                   $"Birth: {reader["birth"]}, Type: {reader["type"]}, Address: {reader["address"]}, " +
                                                   $"Date: {reader["date"]}");
                            }
                            MessageBox.Show(results.ToString(), "Αιτήσης συνολικά");
                        }
                        else
                        {
                            MessageBox.Show("Δεν βρέθηκαν εγγραφές στον πίνακα inf.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Σφάλμα: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string searchName = textBoxSearch.Text;

            if (string.IsNullOrEmpty(searchName))
            {
                MessageBox.Show("Παρακαλώ πληκτρολογήστε ένα όνομα.", "Σφάλμα");
                return;
            }

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                string query = "SELECT * FROM inf WHERE onoma = @name";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", searchName);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            StringBuilder result = new StringBuilder();
                            while (reader.Read())
                            {
                                result.AppendLine($"Onoma: {reader["onoma"]}, Email: {reader["email"]}, Phone: {reader["phone"]}, " +
                                                  $"Birth: {reader["birth"]}, Type: {reader["type"]}, Address: {reader["address"]}, " +
                                                  $"Date: {reader["date"]}");
                            }
                            MessageBox.Show(result.ToString(), $"Αποτελέσματα για τον {searchName}");
                        }
                        else
                        {
                            MessageBox.Show($"Δεν βρέθηκε κανένα άτομο με τοv  {searchName}.", "Αναζήτηση");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Σφάλμα: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string nameToDelete = textBoxSearch2.Text;

            if (string.IsNullOrEmpty(nameToDelete))
            {
                MessageBox.Show("Παρακαλώ πληκτρολογήστε ένα όνομα.", "Σφάλμα");
                return;
            }

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                string deleteQuery = "DELETE FROM inf WHERE onoma = @name";
                using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@name", nameToDelete);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"Η εγγραφή για το άτομο {nameToDelete} διαγράφηκε με επιτυχία.", "Διαγραφή");
                    }
                    else
                    {
                        MessageBox.Show($"Δεν βρέθηκε κανένα άτομο με το όνομα {nameToDelete}.", "Διαγραφή");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Σφάλμα: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string searchName = textBoxSearch3.Text;
            string friendlyFieldName = comboBoxField.SelectedItem?.ToString();
            string newValue = textBoxNewValue.Text;

            if (string.IsNullOrEmpty(searchName))
            {
                MessageBox.Show("Παρακαλώ πληκτρολογήστε ένα όνομα.", "Σφάλμα");
                return;
            }

            if (string.IsNullOrEmpty(friendlyFieldName))
            {
                MessageBox.Show("Παρακαλώ επιλέξτε ένα πεδίο.", "Σφάλμα");
                return;
            }

            if (string.IsNullOrEmpty(newValue))
            {
                MessageBox.Show("Παρακαλώ εισάγετε την νέα τιμή.", "Σφάλμα");
                return;
            }

            string selectedField = fieldMappings[friendlyFieldName];

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                string updateQuery = $"UPDATE inf SET {selectedField} = @newValue WHERE onoma = @name";
                using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@newValue", newValue);
                    command.Parameters.AddWithValue("@name", searchName);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"Το πεδίο '{friendlyFieldName}' ενημερώθηκε με επιτυχία για το άτομο {searchName}.", "Ενημέρωση");
                    }
                    else
                    {
                        MessageBox.Show($"Δεν βρέθηκε κανένα άτομο με το όνομα {searchName}.", "Ενημέρωση");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Σφάλμα: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxField_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string searchValue = textBox8.Text;
            string friendlyColumnName = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(searchValue))
            {
                MessageBox.Show("Παρακαλώ εισάγετε τιμή για αναζήτηση.", "Σφάλμα");
                return;
            }

            if (string.IsNullOrEmpty(friendlyColumnName))
            {
                MessageBox.Show("Παρακαλώ επιλέξτε μια στήλη για αναζήτηση.", "Σφάλμα");
                return;
            }

            string selectedColumn = fieldMappings[friendlyColumnName];

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                string searchQuery = $"SELECT * FROM inf WHERE {selectedColumn} = @searchValue";
                using (SQLiteCommand command = new SQLiteCommand(searchQuery, connection))
                {
                    command.Parameters.AddWithValue("@searchValue", searchValue);

                    SQLiteDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        StringBuilder result = new StringBuilder();
                        while (reader.Read())
                        {
                            result.AppendLine($"Όνομα: {reader["onoma"]}, Email: {reader["email"]}, Phone: {reader["phone"]}, " +
                                $"Birth: {reader["birth"]}, Type: {reader["type"]}, Address: {reader["address"]}, Date: {reader["date"]}");
                        }
                        MessageBox.Show(result.ToString(), "Αποτελέσματα Αναζήτησης");
                    }
                    else
                    {
                        MessageBox.Show("Δεν βρέθηκαν εγγραφές που να ταιριάζουν.", "Αναζήτηση");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Σφάλμα: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string filePath = "applications.txt";

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    string name = textBox1.Text;
                    string email = textBox2.Text;
                    string phone = textBox3.Text;
                    string birth = textBox4.Text;
                    string type = textBox5.Text;
                    string address = textBox6.Text;
                    string date = textBox7.Text;

                    string applicationRecord = $"Όνομα: {name}, Email: {email}, Τηλέφωνο: {phone}, Ημερομηνία Γέννησης: {birth}, Τύπος: {type}, Διεύθυνση: {address}, Ημερομηνία Υποβολής: {date}";

                    writer.WriteLine(applicationRecord);
                }

                MessageBox.Show("Η εγγραφή αποθηκεύτηκε με επιτυχία στο αρχείο applications.txt", "Επιτυχία");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Σφάλμα κατά την αποθήκευση της εγγραφής: {ex.Message}", "Σφάλμα");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            RedirectLink link = new RedirectLink();
            link.Redirect("https://www.gov.gr/");
        }
    }
    public class HelpButton
    {
        public void ShowHelpMessage(object sender, EventArgs e)
        {
            MessageBox.Show("Καλέστε εδώ: 6988234562 για παραπάνω πληροφορίες", "Βοήθεια");
        }
    }

    public class RedirectLink
    {
        public void Redirect(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Σφάλμα κατά το άνοιγμα του συνδέσμου: {ex.Message}", "Σφάλμα", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}