Create DATABASE StudentRecordDB;
USE StudentRecordDB;
Create table Students(
    StudentId int Primary key AUTO_INCREMENT,
    Name varchar(100) not null,
    Age int not null,
    Email varchar(100) not null,
    Department varchar(100) not null,
    Semester int not null
    );
select*from Students;

