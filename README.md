# Web Scraper

```
Author:        Kevin Tran
Date:          November 13, 2020
Course:        CS 4540, University of Utah, School of Computing
Copyright:     CS 4540 and Khoi Minh Tran - This work may not be copied for use in Academic Coursework.
```

### Find Course Enrollments with this format: Course Dept,Course Number,Course Credits,Course Title,Course Enrollment,Course Semester,Course Year,Course Description 
* First you will need to put in Semester, Year, Subject and limit
* The semester need to be 3 of these strings: fall, spring, summer, you can put it uppercase or lowercase, either case should be handled in the application.
* The year will be the year which you would like to search in this form (e.g, 2020, 2019, etc.) The year cannot be exceed greater than 2021 since it does not exceed yet.
* The application will throw errors if the year put in wrong format.
The subject would be a string with 2 characters only for example "CS".
* The limit would be display the courses limit to the number that you put in the box. For example, if you put in "10" it will limit the display to be only 10 courses.
Furthermore, the limit need to be at least greater than or equal to 1. Otherwise, it will show you error messages.
(Please allow up to 30 seconds for the error message to pop up)

Once you click "Navigate to Courses" button it will automatically lead you to the page and show you the appropriate data in the lower textbox.
You can also save the data as csv file by clicking the "Save" button. The location of the csv will be in PS7\bin\Debug\netcoreapp3.1\list.csv

### Find Course Catalog Description:
* First you need to put in the course which you would like to search. For example, CS4400, CS4540, etc.
If you put in invalid code the application will pop up said that it couldn't find that course. 
(Please allow up to 30 seconds for the windows to pop up)
* Click "Get Course Description" button and it will pop up a window with the Course which you just put in.

### Save data
* When you click to the "Save" button it will automatically save the data in PS7\bin\Debug\netcoreapp3.1\list.csv
