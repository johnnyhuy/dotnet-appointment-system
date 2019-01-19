create table Room
(
    RoomID nvarchar(10) not null,
    constraint PK_Room primary key (RoomID)
);

create table StudentUser
(
    StudentID nvarchar(8) not null,
    Name nvarchar(20) not null,
    Email nvarchar(20) not null,
    constraint PK_User primary key (StudentID)
);
create table StaffUser
(
    StaffID nvarchar(8) not null,
    Name nvarchar(20) not null,
    Email nvarchar(20) not null,
    constraint PK_User primary key (StaffID)
);

create table Slot
(
    RoomID nvarchar(10) not null,
    StartTime datetime not null,
    StaffID nvarchar(8) not null,
    BookedInStudentID nvarchar(8) null,
    constraint PK_Slot primary key (RoomID, StartTime),
    constraint FK_Slot_Room foreign key (RoomID) references Room (RoomID),
    constraint FK_Slot_User_Staff foreign key (StaffID)
    references User (UserID),
    constraint FK_Slot_User_Student foreign key (BookedInStudentID)
    references User (UserID)
);
