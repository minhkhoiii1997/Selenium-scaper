/*Author: Kevin Tran
  Date: 11 / 13 / 2020
  Course: CS 4540, University of Utah, School of Computing
  Copyright: CS 4540 and Kevin Tran. This work may not be copied for use in Academic Coursework.

  I, Kevin Tran certify that I wrote this code from scratch and did not copy it in part or whole from
  another source.  Any references used in the completion of the assignment are cited in my README file.

  File Contents
  HTML structure for the student's view of the URC when viewing the details of a research opportunity
*/

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS7
{
    public partial class Form1 : Form
    {
        //Global variables
        private ChromeDriver _driver;
        private List<string> trText;
        private int limit;

        public Form1()
        {
            InitializeComponent();
        }

        //Get Course description from https://catalog.utah.edu
        private void button4_Click(object sender, EventArgs e)
        {
            //connect to Selenium
            _driver = new ChromeDriver();
            // check if catalog textbox is empty
            if (String.IsNullOrEmpty(courseSearch.Text))
            {
                MessageBox.Show("Please put in course with the format (e.g, CS4540, CS4400, etc.");
            }
            else
            {
                try
                {
                    textBox1.Text += "Please wait while you are being redirect to the website..." + "\r\n";
                    _driver.Navigate().GoToUrl("https://catalog.utah.edu");
                    //wait for at most 30 seconds until proceed to the next steps. Reference https://www.lambdatest.com/blog/implicit-wait-csharp-selenium/
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                    _driver.FindElementByLinkText("Courses").Click();
                    // send the course which you would like to search
                    _driver.FindElementById("Search").SendKeys(courseSearch.Text);
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                    var table = _driver.FindElementByClassName("_2Fvl8L1U");
                    // click on the course search result
                    table.FindElement(By.TagName("a")).Click();

                    //Find the course description text
                    var desc = _driver.FindElement(By.XPath("//*[text()='Course Description']/following-sibling::div/div/div/div/div")).Text;
                    textBox1.Text += "Please refer to the Message Box for course description." + "\r\n";
                    //display Course Description to user
                    MessageBox.Show(desc);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Couldn't find the course)\n");
                }
            }

        }

        // Get all the data and store it in a list for saving the data to csv.
        private void button5_Click(object sender, EventArgs e)
        {
            //Connect to Selenium
            _driver = new ChromeDriver();
            trText = new List<string>();
            // Check if users put in year, semester, or subject
            // to the textbox. If not show a message to inform 
            // user put those in before progressing
            if (String.IsNullOrEmpty(semester.Text) || String.IsNullOrEmpty(year.Text) || String.IsNullOrEmpty(subject.Text) || String.IsNullOrEmpty(number.Text))
            {
                MessageBox.Show("Please type in semester, year, subject, limit");
            }
            else
            {

                string url = "";
                //checking the input of the user then put in the appropriate data from user's input
                if (year.Text.Length != 4 && Int32.Parse(year.Text) >= 2021)
                {
                    MessageBox.Show("Year should be 4 characters, and should be <= 2021. Please check again");
                    return;
                }
                else
                {
                    url += "1" + (Int32.Parse(year.Text) - 2000).ToString().PadLeft(2, '0');
                }
                //checking the input of the user for semester.
                switch (semester.Text.ToLower())
                {
                    case "spring":
                        url += "4";
                        break;
                    case "summer":
                        url += "6";
                        break;
                    case "fall":
                        url += "8";
                        break;
                    default:
                        MessageBox.Show("Invalid semester input, please check again");
                        return;
                }
                if (subject.Text.Length != 2)
                {
                    MessageBox.Show("Your subject should have exactly 2 characters. Please try it again.");
                    return;
                }
                if (Int32.Parse(number.Text) <= 0)
                {
                    MessageBox.Show("The number of course cannot be 0. Please try it again");
                    return;
                }
                limit = Int32.Parse(number.Text);
                textBox1.Text += "Please wait while we collect your data..." + "\r\n";
                //Go to the link 
                _driver.Navigate().GoToUrl("https://student.apps.utah.edu/uofu/stu/ClassSchedules/main/" + url + "/class_list.html?subject=" + subject.Text.ToUpper());
                // Wait for couple seconds for the website to load
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                _driver.FindElementByLinkText("Seating availability for all CS classes").Click();

                _driver.FindElementById("seatingAvailabilityTable");
                var body = _driver.FindElement(By.TagName("tbody"));

                var trs = body.FindElements(By.TagName("tr"));

                int i = 0;
                foreach (var element in trs)
                {
                    var tds = element.FindElements(By.TagName("td"));
                    var course_dep = tds.ElementAt(1).Text;
                    var course_number = tds.ElementAt(2).Text;
                    var section = tds.ElementAt(3).Text;
                    var title = tds.ElementAt(4).Text;
                    var enroll = tds.ElementAt(5).Text;

                    if (section == "001" && Int32.Parse(course_number) >= 1000 && Int32.Parse(course_number) <= 7000 && !title.Contains("Seminar") && !title.Contains("Special Topics"))
                    {
                        /*while(i < Int32.Parse(number.Text))
                        {*/
                            trText.Add(course_dep + "," + course_number + "," + section + "," + title + "," + enroll);
                            i++;
                        //}
                    }
                }
                for (var j = 0; j < limit; j++)
                {
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                    string line = trText[j];
                    string[] words = line.Split(',');
                    string course_dep = words[0];
                    string course_number = words[1];
                    string section = words[2];
                    string title = words[3];
                    string enroll = words[4];
                    _driver.Navigate().GoToUrl("https://student.apps.utah.edu/uofu/stu/ClassSchedules/main/" + url + "/description.html?subj=" + subject.Text + "&catno=" + course_number + "&section=001");
                    var headers = _driver.FindElementsByClassName("card-header");
                    string unit = "";
                    string course_desc = "";
                    for (var ee = 0; ee < headers.Count; ee++)
                    {
                        if (headers.ElementAt(ee).Text == "Course Detail")
                        {
                            unit = _driver.FindElementsByClassName("card-body").ElementAt(ee).FindElement(By.ClassName("col")).Text;
                        }
                        else if (headers.ElementAt(ee).Text == "Description")
                        {
                            course_desc = _driver.FindElementsByClassName("card-body").ElementAt(ee).FindElement(By.ClassName("col")).Text;
                        }
                    }
                    trText[j] = course_dep + "," + course_number + "," + unit + "," + title + "," + enroll + "," + semester.Text + "," + year.Text + ",\"" + course_desc + "\"";
                    textBox1.Text += trText[j] + "\r\n";
                }
                textBox1.Text += "Finish collecting all data. You can save it by clicking on the Save button." + "\r\n";
            }
        }

        // Save the list of data to csv file. Reference https://stackoverflow.com/questions/32466674/write-in-next-column-in-csv-file-c-sharp
        private void saveToCSV(List<string> list)
        {
            using (StreamWriter sw = File.CreateText("list.csv"))
            {
                // set header
                sw.WriteLine("Course Dept,Course Number,Course Credits,Course Title,Course Enrollment,Course Semester,Course Year,Course Description");
                sw.Flush();
                for (int i = 0; i < limit; i++)
                {
                    sw.WriteLine(list[i]);
                    sw.Flush();
                }
                sw.Close();
            }
        }

        //Click save button
        private void button6_Click(object sender, EventArgs e)
        {
            saveToCSV(trText);
            textBox1.Text += "Successfully save data to csv file." + "\r\n";
        }
    }
}
