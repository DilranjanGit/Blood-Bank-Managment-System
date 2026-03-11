--Create Database BBMS

Use BBMS
/*
CREATE TABLE Roles (
    RoleId INT IDENTITY(1,1) PRIMARY KEY,
    RoleName VARCHAR(50) NOT NULL UNIQUE
);



CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FullName VARCHAR(150) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(MAX) NOT NULL,
    Phone VARCHAR(20),
    RoleId INT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId)
        REFERENCES Roles(RoleId)
);



CREATE TABLE Donors (
    DonorId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL UNIQUE,
    BloodGroup VARCHAR(5) NOT NULL,
    Age INT NOT NULL,
    Gender VARCHAR(10),
    Address VARCHAR(250),

    CONSTRAINT FK_Donors_Users FOREIGN KEY (UserId)
        REFERENCES Users(UserId)
);



CREATE TABLE BloodDonations (
    DonationId INT IDENTITY(1,1) PRIMARY KEY,
    DonorId INT NOT NULL,
    DonationDate DATETIME NOT NULL DEFAULT GETDATE(),
    RBCCount DECIMAL(10,2),
    WBCCount DECIMAL(10,2),
    PlateletsCount DECIMAL(10,2),
    VolumeInML INT NOT NULL,
    Status VARCHAR(50) NOT NULL,

    CONSTRAINT FK_BloodDonations_Donors FOREIGN KEY (DonorId)
        REFERENCES Donors(DonorId)
);



CREATE TABLE BloodProducts (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    ProductName VARCHAR(100) NOT NULL,
    Description VARCHAR(250)
);




CREATE TABLE Inventory (
    InventoryId INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT NOT NULL,
    BloodGroup VARCHAR(5) NOT NULL,
    QuantityAvailable INT NOT NULL DEFAULT 0,
    LastUpdated DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Inventory_Products FOREIGN KEY (ProductId)
	REFERENCES BloodProducts(ProductID)
	);

	

	CREATE TABLE Orders (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    BloodGroup VARCHAR(5) NOT NULL,
    DeliveryDate DATETIME NOT NULL,
    DeliveryAddress VARCHAR(250) NOT NULL,
    TotalAmount DECIMAL(10,2) NOT NULL,
    OrderStatus VARCHAR(50) NOT NULL DEFAULT 'Pending',
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Orders_Users FOREIGN KEY (UserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_Orders_Products FOREIGN KEY (ProductId)
        REFERENCES BloodProducts(ProductId)
);



CREATE TABLE Payments (
    PaymentId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    PaymentMethod VARCHAR(50) NOT NULL,
    PaymentGateway VARCHAR(50),
    TransactionId VARCHAR(100),
    AmountPaid DECIMAL(10,2) NOT NULL,
    PaymentStatus VARCHAR(50) NOT NULL,
    PaymentDate DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Payments_Orders FOREIGN KEY (OrderId)
        REFERENCES Orders(OrderId)
);



CREATE TABLE OrderStatusHistory (
    HistoryId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    Status VARCHAR(50) NOT NULL,
    UpdatedBy INT NOT NULL,
    UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_StatusHistory_Orders FOREIGN KEY (OrderId)
        REFERENCES Orders(OrderId),

    CONSTRAINT FK_StatusHistory_Users FOREIGN KEY (UpdatedBy)
        REFERENCES Users(UserId)
);

CREATE TABLE Notifications (
    NotificationId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Message VARCHAR(MAX) NOT NULL,
    Type VARCHAR(20) NOT NULL,
    SentAt DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Notifications_Users FOREIGN KEY (UserId)
        REFERENCES Users(UserId)
);

*/

