CREATE DATABASE BankingDB;
GO
USE BankingDB;
GO


CREATE TABLE Admin
(
    Admin_Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Email_Id VARCHAR(100) UNIQUE NOT NULL,
    Password VARCHAR(200) NOT NULL,
    Created_At DATETIME DEFAULT GETDATE()
);

CREATE TABLE AccountRequest
(
    Service_Reference_Number INT IDENTITY(10000,1) PRIMARY KEY,
    Title VARCHAR(5),
    First_Name VARCHAR(50) NOT NULL,
    Middle_Name VARCHAR(50),
    Last_Name VARCHAR(50) NOT NULL,
    Father_Name VARCHAR(50) NOT NULL,
    Gender VARCHAR(10) NOT NULL,
    Date_Of_Birth DATE NOT NULL,
    Mobile_Number BIGINT NOT NULL,
    Email_Id VARCHAR(100) UNIQUE NOT NULL,
    Aadhar CHAR(12) UNIQUE NOT NULL,
    Residential_Address VARCHAR(200) NOT NULL,
    Permanent_Address VARCHAR(200) NOT NULL,
    Occupation_Type VARCHAR(50),
    Source_Of_Income VARCHAR(50),
    Gross_Annual_Income DECIMAL(18,2),
    Opt_Debit_Card BIT DEFAULT 0,
    Opt_Net_Banking BIT DEFAULT 0,
    Status VARCHAR(20) DEFAULT 'PENDING',  -- PENDING / APPROVED / REJECTED
    Requested_At DATETIME DEFAULT GETDATE(),
    Approved_At DATETIME
);

CREATE TABLE Customer
(
    Customer_Id INT IDENTITY(10000,1) PRIMARY KEY,
    Service_Reference_Number INT UNIQUE,
    Full_Name VARCHAR(150),
    Mobile_Number BIGINT,
    Email_Id VARCHAR(100),
    Aadhar CHAR(12),
    Created_At DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (Service_Reference_Number)
    REFERENCES AccountRequest(Service_Reference_Number)
);


CREATE TABLE Accounts
(
    Account_Number BIGINT IDENTITY(5000000001,1) PRIMARY KEY,
    Customer_Id INT NOT NULL,
    Account_Type VARCHAR(20) DEFAULT 'SAVINGS',
    Balance DECIMAL(18,2) DEFAULT 1000,
    Is_Active BIT DEFAULT 1,
    Created_At DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (Customer_Id)
    REFERENCES Customer(Customer_Id)
);


CREATE TABLE InternetBanking
(
    User_Id VARCHAR(30) PRIMARY KEY,
    Customer_Id INT UNIQUE,
    Account_Number BIGINT UNIQUE,
    Login_Password VARCHAR(200) NOT NULL,
    Transaction_Password VARCHAR(200) NOT NULL,
    Failed_Attempts INT DEFAULT 0,
    Is_Locked BIT DEFAULT 0,
    Locked_Time DATETIME,
    Last_Login DATETIME,

    FOREIGN KEY (Customer_Id)
    REFERENCES Customer(Customer_Id),

    FOREIGN KEY (Account_Number)
    REFERENCES Accounts(Account_Number)
);


CREATE TABLE OtpRequests
(
    Otp_Id INT IDENTITY(1,1) PRIMARY KEY,
    User_Id VARCHAR(30),
    Otp CHAR(6),
    Created_Time DATETIME DEFAULT GETDATE(),
    Expires_At DATETIME,
    Is_Used BIT DEFAULT 0,

    FOREIGN KEY (User_Id)
    REFERENCES InternetBanking(User_Id)
);


CREATE TABLE Payees
(
    Payee_Id INT IDENTITY(1,1) PRIMARY KEY,
    User_Id VARCHAR(30),
    Beneficiary_Name VARCHAR(100) NOT NULL,
    Beneficiary_Account_Number BIGINT NOT NULL,
    Nickname VARCHAR(50),
    Created_At DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (User_Id)
    REFERENCES InternetBanking(User_Id)
);


CREATE TABLE Transactions
(
    Transaction_Id BIGINT IDENTITY(10000,1) PRIMARY KEY,
    From_Account BIGINT,
    To_Account BIGINT,
    Transaction_Mode VARCHAR(10),   -- NEFT / RTGS / IMPS
    Transaction_Type VARCHAR(10),   -- CREDIT / DEBIT
    Amount DECIMAL(18,2) CHECK (Amount > 0),
    Transaction_Date DATETIME DEFAULT GETDATE(),
    Status VARCHAR(20),              -- SUCCESS / FAILED
    Remarks VARCHAR(100),

    FOREIGN KEY (From_Account)
    REFERENCES Accounts(Account_Number),

    FOREIGN KEY (To_Account)
    REFERENCES Accounts(Account_Number)
);


CREATE TABLE DebitCards
(
    Debit_Card_Number BIGINT IDENTITY(4000000000000000,1) PRIMARY KEY,
    Account_Number BIGINT UNIQUE,
    Expiry_Date DATE,
    CVV INT,
    Is_Active BIT DEFAULT 1,

    FOREIGN KEY (Account_Number)
    REFERENCES Accounts(Account_Number)
);


CREATE TABLE SupportMessages
(
    Message_Id INT IDENTITY(1,1) PRIMARY KEY,
    User_Id VARCHAR(30),
    Subject VARCHAR(200),
    Message TEXT,
    Sent_At DATETIME DEFAULT GETDATE(),
    Admin_Reply TEXT,
    Replied_At DATETIME,
    Status VARCHAR(20) DEFAULT 'PENDING',

    FOREIGN KEY (User_Id)
    REFERENCES InternetBanking(User_Id)
);








