-- Create Database Western Sydney Medical Practice ----------------
--					Author: Peter Panagopoulos  19/10/2020
-- ****************************************************************
GO
USE MASTER
GO
IF EXISTS(	SELECT	name
			FROM	master..sysdatabases
			WHERE	name = N'WestSydMedPrac_SAND'
		 )
DROP DATABASE	WestSydMedPrac_SAND;
GO
CREATE DATABASE WestSydMedPrac_SAND;
GO
USE WestSydMedPrac_SAND;
GO
EXEC sp_changedbowner 'sa'
GO
CREATE TABLE PATIENT(
Patient_ID		INT	IDENTITY(1,1),
Gender			NCHAR(1),
DateOfBirth		DATE,
FirstName		NVARCHAR(50),
LastName		NVARCHAR(50),
Street			NVARCHAR(30),		--Mamungkukumpurangkuntjunya is a hill in the Aussie Outback. 
									--It is the longest official place name in Australia, with 26 letters.
									--The name means "where the devil urinates" in the local Pitjantjatjara language.
Suburb			NVARCHAR(30),
[State]			NVARCHAR(3),		-- NSW, QLD, WA, ACT etc.
PostCode		NCHAR(4),
HomePhone		NVARCHAR(10),
Mobile			NVARCHAR(10),
MedicareNumber	NVARCHAR(15) NULL,
Notes			NVARCHAR(MAX)
CONSTRAINT patient_pk PRIMARY KEY (Patient_ID)
);
GO
CREATE TABLE PRACTITIONER_TYPE(
PractnrTypeName	NVARCHAR(20)
CONSTRAINT practitioner_type_pk PRIMARY KEY (PractnrTypeName)
);
GO
--CREATE TABLE WEEKDAYS(
--Day_Name				NVARCHAR(9),
--DayOrder				TINYINT
--CONSTRAINT weekdays_pk	PRIMARY KEY (Day_Name)
--);
GO
CREATE TABLE PRACTITIONER(
Practitioner_ID	INT IDENTITY(1,1),
FirstName			NVARCHAR(50),
LastName			NVARCHAR(50),
Street				NVARCHAR(30),		
Suburb				NVARCHAR(30),
[State]				NVARCHAR(3),		
PostCode			NCHAR(4),
HomePhone			NVARCHAR(10),
Mobile				NVARCHAR(10),
RegistrationNumber	NVARCHAR(15),
PractnrTypeName_Ref	NVARCHAR(20) NOT NULL DEFAULT 'Not Assigned',
Monday				BIT,
Tuesday				BIT,
Wednesday			BIT,
Thursday			BIT,
Friday				BIT,
Saturday			BIT,
Sunday				BIT
CONSTRAINT practitioner_pk PRIMARY KEY (Practitioner_ID),
CONSTRAINT prac_type_fk FOREIGN KEY (PractnrTypeName_Ref) REFERENCES PRACTITIONER_TYPE
ON UPDATE CASCADE ON DELETE SET DEFAULT
);
GO
--CREATE TABLE AVAILABLE(
--Day_Name_Ref		NVARCHAR(9),
--Practitioner_Ref	INT
--CONSTRAINT available_pk PRIMARY KEY (Day_Name_Ref, Practitioner_Ref),
--CONSTRAINT avialable_prac_fk FOREIGN KEY (Practitioner_Ref) REFERENCES PRACTITIONER,
--CONSTRAINT available_weekdays_fk FOREIGN KEY (Day_Name_Ref) REFERENCES WEEKDAYS
--ON UPDATE NO ACTION ON DELETE NO ACTION  -- Not allowed to change or delete the days of the week.
--);
--GO
CREATE TABLE TIMESLOT(
StartTime			TIME,
EndTime				TIME,
TimeSlot			NVARCHAR(100)
CONSTRAINT timeslot_pk PRIMARY KEY (StartTime)
);
GO
CREATE TABLE APPOINTMENT(
Practitioner_Ref	INT,
AppointmentDate		DATE,
AppointmentTime_Ref	TIME,
Patient_Ref			INT,
CONSTRAINT appointment_pk PRIMARY KEY (Practitioner_Ref, AppointmentDate, AppointmentTime_Ref),
CONSTRAINT appoint_prac_fk FOREIGN KEY (Practitioner_Ref) REFERENCES PRACTITIONER
ON UPDATE CASCADE ON DELETE CASCADE,
CONSTRAINT appoint_patient_fk FOREIGN KEY (Patient_Ref) REFERENCES PATIENT
ON UPDATE CASCADE ON DELETE CASCADE,
CONSTRAINT appoint_timeslot_fk FOREIGN KEY (AppointmentTime_Ref) REFERENCES TIMESLOT,
CONSTRAINT appointment_Unique  UNIQUE (AppointmentDate, AppointmentTime_Ref, Patient_Ref)			-- Patient can't be double booked for an appointment.
);
GO
-- ======================================================================
--					test data
-- ======================================================================
SET DATEFORMAT DMY;
GO
INSERT INTO PATIENT (Gender, DateOfBirth, FirstName, LastName, Street, Suburb, [State], PostCode, HomePhone, Mobile, MedicareNumber, Notes) VALUES('M', '23/02/1991', 'Paul', 'Smith', '12 Jockson Street', 'Brightwaters', 'NSW', '2264', '0249765473','0402456654', '365243546352435', 'Paul has had problems with his lower abdomin.');
INSERT INTO PATIENT (Gender, DateOfBirth, FirstName, LastName, Street, Suburb, [State], PostCode, HomePhone, Mobile, MedicareNumber, Notes) VALUES('M', '04/09/1953', 'John', 'Honest', '123 Dandaraga Road', 'Mirrabooka', 'NSW', '2264', '0249736453','0403556655', '645738563527483', 'John has diabetes');
INSERT INTO PATIENT (Gender, DateOfBirth, FirstName, LastName, Street, Suburb, [State], PostCode, HomePhone, Mobile, MedicareNumber, Notes) VALUES('F', '12/12/1974', 'Linda', 'Ronstadh', '344 Simspon Road', 'Cooranbong', 'NSW', '2264', '024973654','0405321345', '976846352746378', 'Linda has ongoing coughing fits and high blood pressure');
INSERT INTO PATIENT (Gender, DateOfBirth, FirstName, LastName, Street, Suburb, [State], PostCode, HomePhone, Mobile, MedicareNumber, Notes) VALUES('M', '15/03/1969', 'Barry', 'Bloomberg', '65 Brown Street', 'Newtown', 'NSW', '2060', '0296602233','0404543278', '454543233445555', 'Barry has high blood pressure and diabetes.');
INSERT INTO PATIENT (Gender, DateOfBirth, FirstName, LastName, Street, Suburb, [State], PostCode, HomePhone, Mobile, MedicareNumber, Notes) VALUES('F', '06/08/1991', 'Amy', 'Weinhouse', '44 Jockson Street', 'Brightwaters', 'NSW', '2264', '0249768545','0403374654', '561443546879435', 'Amy has a heroin addiction and is currently on the methodone program');
INSERT INTO PATIENT (Gender, DateOfBirth, FirstName, LastName, Street, Suburb, [State], PostCode, HomePhone, Mobile, MedicareNumber, Notes) VALUES('M', '25/04/1979', 'Jack', 'Jones', 'Unit 4/ 32 Parramatta Road', 'Lidcombe', 'NSW', '2434', '0298755478','9564789304', '674859472108475', 'Jack has low blood pressure and anemia.');
INSERT INTO PATIENT (Gender, DateOfBirth, FirstName, LastName, Street, Suburb, [State], PostCode, HomePhone, Mobile, MedicareNumber, Notes) VALUES('F', '14/09/1993', 'Marry', 'Smith', '22 Church Street', 'Camperdown', 'NSW', '2050', '0296602223','0147598246', '521463279546137', 'Marry is health so far and just comes in for check ups.');
GO
INSERT INTO PRACTITIONER_TYPE (PractnrTypeName) VALUES('Doctor GP');
INSERT INTO PRACTITIONER_TYPE (PractnrTypeName) VALUES('Nurse');
GO
-- ***********************************************************************************
-- ======================Creating TimeSlots of 30 minute intervals ===================
DECLARE @TIMESLOT TABLE
     (StartTime TIME, EndTime TIME, TimeSlot NVARCHAR(100))

INSERT INTO @TIMESLOT
SELECT '08:30', '17:30', '08:30AM - 05:30PM'
 UNION ALL 
 SELECT '17:30', '18:00', '5:30PM - 6:00PM';

WITH Tally (n) AS (
    SELECT TOP 100 30*(ROW_NUMBER() OVER (ORDER BY (SELECT NULL))-1)
    FROM sys.all_columns)
INSERT INTO TIMESLOT
SELECT	TSStart=DATEADD(minute, n, StartTime),
		TSEnd=DATEADD(minute, n + 30, StartTime),
    	TimeSlot=CONVERT(VARCHAR(100), DATEADD(minute, n, StartTime), 0) + ' - ' +
        CONVERT(VARCHAR(100), DATEADD(minute, n + 30, StartTime), 0)
FROM @TIMESLOT
CROSS APPLY (
    SELECT n 
    FROM Tally 
    WHERE n BETWEEN 0 AND DATEDIFF(minute, StartTime, DATEADD(minute, -30, EndTime))) a
ORDER BY  TSStart
-- =====================================================================================
-- *************************************************************************************
GO
--INSERT INTO WEEKDAYS (Day_Name, DayOrder) VALUES ('Monday', 1);
--INSERT INTO WEEKDAYS (Day_Name, DayOrder) VALUES ('Tuesday', 2);
--INSERT INTO WEEKDAYS (Day_Name, DayOrder) VALUES ('Wednesday', 3);
--INSERT INTO WEEKDAYS (Day_Name, DayOrder) VALUES ('Thursday', 4);
--INSERT INTO WEEKDAYS (Day_Name, DayOrder) VALUES ('Friday', 5);
--INSERT INTO WEEKDAYS (Day_Name, DayOrder) VALUES ('Saturday', 6);
--INSERT INTO WEEKDAYS (Day_Name, DayOrder) VALUES ('Sunday', 7);
--GO
SET IDENTITY_INSERT [dbo].[PRACTITIONER] ON
INSERT INTO [dbo].[PRACTITIONER] ([Practitioner_ID], [FirstName], [LastName], [Street], [Suburb], [State], [PostCode], [HomePhone], [Mobile], [RegistrationNumber], [PractnrTypeName_Ref], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday], [Saturday], [Sunday]) VALUES (1, N'Peter', N'Panagopoulos', N'12 Jockson Street', N'Brightwaters', N'NSW', N'2264', N'0249765473', N'0402456654', N'365243546352435', N'Doctor GP', 1, 0, 1, 0, 1, 0, 0)
INSERT INTO [dbo].[PRACTITIONER] ([Practitioner_ID], [FirstName], [LastName], [Street], [Suburb], [State], [PostCode], [HomePhone], [Mobile], [RegistrationNumber], [PractnrTypeName_Ref], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday], [Saturday], [Sunday]) VALUES (2, N'Jerry', N'Hornery', N'123 Dandaraga Road', N'Mirrabooka', N'NSW', N'2264', N'0249736453', N'0403556655', N'645738563527483', N'Doctor GP', 0, 1, 0, 1, 1, 0, 0)
INSERT INTO [dbo].[PRACTITIONER] ([Practitioner_ID], [FirstName], [LastName], [Street], [Suburb], [State], [PostCode], [HomePhone], [Mobile], [RegistrationNumber], [PractnrTypeName_Ref], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday], [Saturday], [Sunday]) VALUES (3, N'Leanne', N'Rothsteine', N'344 Simspon Road', N'Cooranbong', N'NSW', N'2264', N'024973654', N'0405321345', N'976846352746378', N'Doctor GP', 1, 0, 1, 0, 1, 0, 0)
INSERT INTO [dbo].[PRACTITIONER] ([Practitioner_ID], [FirstName], [LastName], [Street], [Suburb], [State], [PostCode], [HomePhone], [Mobile], [RegistrationNumber], [PractnrTypeName_Ref], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday], [Saturday], [Sunday]) VALUES (4, N'Barnaby', N'Blight', N'65 Brown Street', N'Newtown', N'NSW', N'2060', N'0296602233', N'0404543278', N'454543233445555', N'Nurse', 1, 0, 1, 0, 1, 0, 0)
INSERT INTO [dbo].[PRACTITIONER] ([Practitioner_ID], [FirstName], [LastName], [Street], [Suburb], [State], [PostCode], [HomePhone], [Mobile], [RegistrationNumber], [PractnrTypeName_Ref], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday], [Saturday], [Sunday]) VALUES (5, N'Aritha', N'Franklin', N'44 Jockson Street', N'Brightwaters', N'NSW', N'2264', N'0249768545', N'0403374654', N'561443546879435', N'Nurse', 0, 1, 0, 1, 1, 0, 0)
INSERT INTO [dbo].[PRACTITIONER] ([Practitioner_ID], [FirstName], [LastName], [Street], [Suburb], [State], [PostCode], [HomePhone], [Mobile], [RegistrationNumber], [PractnrTypeName_Ref], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday], [Saturday], [Sunday]) VALUES (6, N'Jospeph', N'Jilliby', N'Unit 4/ 32 Parramatta Road', N'Lidcombe', N'NSW', N'2434', N'0298755478', N'9564789304', N'674859472108475', N'Nurse', 1, 0, 1, 0, 1, 0, 0)
INSERT INTO [dbo].[PRACTITIONER] ([Practitioner_ID], [FirstName], [LastName], [Street], [Suburb], [State], [PostCode], [HomePhone], [Mobile], [RegistrationNumber], [PractnrTypeName_Ref], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday], [Saturday], [Sunday]) VALUES (7, N'Marriam', N'Saroka', N'22 Church Street', N'Camperdown', N'NSW', N'2050', N'0296602223', N'0147598246', N'521463279546137', N'Nurse', 0, 1, 0, 1, 1, 0, 0)
SET IDENTITY_INSERT [dbo].[PRACTITIONER] OFF

--GO
--INSERT INTO AVAILABLE VALUES ('Monday', 1);
--INSERT INTO AVAILABLE VALUES ('Wednesday', 1);
--INSERT INTO AVAILABLE VALUES ('Friday', 1);
--INSERT INTO AVAILABLE VALUES ('Monday', 4);
--INSERT INTO AVAILABLE VALUES ('Wednesday', 4);
--INSERT INTO AVAILABLE VALUES ('Friday', 4);
--INSERT INTO AVAILABLE VALUES ('Monday', 6);
--INSERT INTO AVAILABLE VALUES ('Wednesday', 6);
--INSERT INTO AVAILABLE VALUES ('Friday', 6);
--INSERT INTO AVAILABLE VALUES ('Tuesday', 2);
--INSERT INTO AVAILABLE VALUES ('Wednesday', 2);
--INSERT INTO AVAILABLE VALUES ('Thursday', 2);
--INSERT INTO AVAILABLE VALUES ('Tuesday', 5);
--INSERT INTO AVAILABLE VALUES ('Wednesday', 5);
--INSERT INTO AVAILABLE VALUES ('Thursday', 5);
--INSERT INTO AVAILABLE VALUES ('Tuesday', 7);
--INSERT INTO AVAILABLE VALUES ('Wednesday', 7);
--INSERT INTO AVAILABLE VALUES ('Thursday', 7);
--INSERT INTO AVAILABLE VALUES ('Monday', 3);
--INSERT INTO AVAILABLE VALUES ('Tuesday', 3);
--INSERT INTO AVAILABLE VALUES ('Wednesday', 3);
--INSERT INTO AVAILABLE VALUES ('Thursday', 3);
--INSERT INTO AVAILABLE VALUES ('Friday', 3);
GO
-- Should NOT be able to double book a Patient EITHER!!!
SET DATEFORMAT DMY;
INSERT INTO APPOINTMENT VALUES(1, '21/11/2017', '8:30 AM', 1);
INSERT INTO APPOINTMENT VALUES(1, '21/11/2017', '9:00 AM', 2);
INSERT INTO APPOINTMENT VALUES(1, '21/11/2017', '9:30 AM', 3);
INSERT INTO APPOINTMENT VALUES(1, '21/11/2017', '10:00 AM', 4);
INSERT INTO APPOINTMENT VALUES(1, '21/11/2017', '11:30 AM', 5);
INSERT INTO APPOINTMENT VALUES(1, '21/11/2017', '12:00 PM', 1);
INSERT INTO APPOINTMENT VALUES(1, '21/11/2017', '12:30 PM', 7);
INSERT INTO APPOINTMENT VALUES(3, '28/11/2017', '8:30 AM', 1);
INSERT INTO APPOINTMENT VALUES(3, '28/11/2017', '9:00 AM', 3);
INSERT INTO APPOINTMENT VALUES(3, '28/11/2017', '9:30 AM', 6);
INSERT INTO APPOINTMENT VALUES(3, '28/11/2017', '10:00 AM', 4);
INSERT INTO APPOINTMENT VALUES(3, '28/11/2017', '10:30 AM', 5);
GO
-- =======================UPDATE PATIENTS========================
-- **************************************************************
CREATE PROCEDURE [dbo].[usp_UpdatePatient]
	@Patient_ID	INTEGER,
	@Gender		NCHAR(1),
	@DateOfBirth	DATE,
	@FirstName	NVARCHAR(50),
	@LastName	NVARCHAR(50),
	@Street		NVARCHAR(30),
	@Suburb		NVARCHAR(30),
	@State		NVARCHAR(3),
	@PostCode	NCHAR(4),
	@HomePhone	NVARCHAR(10),
	@Mobile		NVARCHAR(10),
	@MedicareNumber	NVARCHAR(15),
	@Notes		NVARCHAR(MAX)
AS
	-- Return the number of records affected
	SET NOCOUNT OFF;
BEGIN
	UPDATE	PATIENT 
	SET		
			Gender = @Gender,
			DateOfBirth = @DateOfBirth,
			FirstName = @FirstName,
			LastName = @LastName,
			Street = @Street,
			Suburb = @Suburb,
			[State] = @State,
			PostCode = @PostCode,
			HomePhone = @HomePhone,
			Mobile = @Mobile,
			MedicareNumber = @MedicareNumber,
			Notes = @Notes		
	WHERE Patient_ID = @Patient_ID
END
GO
-- =======================GET PATIENTS======================================
-- *************************************************************************
CREATE PROCEDURE [dbo].[usp_GetPatient]
	@Patient_ID INT
AS
BEGIN
	SELECT * 
	FROM	PATIENT
	WHERE	Patient_ID = @Patient_ID 
END
GO
-- =======================DELETE PATIENT======================================
-- ***************************************************************************
CREATE PROCEDURE [dbo].[usp_DeletePatient]
	@Patient_ID INTEGER
AS
	SET NOCOUNT OFF;
	BEGIN

	DELETE FROM PATIENT WHERE Patient_ID = @Patient_ID;

	END
-- ===========================================================================
GO
-- ========================INSERT PATIENT=====================================
-- ***************************************************************************
CREATE PROCEDURE [dbo].[usp_InsertPatient]
	@Gender		NCHAR(1),
	@DateOfBirth	DATE,
	@FirstName	NVARCHAR(50),
	@LastName	NVARCHAR(50),
	@Street		NVARCHAR(30),
	@Suburb		NVARCHAR(30),
	@State		NVARCHAR(3),
	@PostCode	NCHAR(4),
	@HomePhone	NVARCHAR(10),
	@Mobile		NVARCHAR(10),
	@MedicareNumber	NVARCHAR(15),
	@Notes		NVARCHAR(MAX)
AS
	SET NOCOUNT OFF;
	BEGIN
	INSERT INTO PATIENT (Gender, DateOfBirth, FirstName, LastName, Street, Suburb, [State], PostCode, HomePhone, Mobile, MedicareNumber, Notes)
				VALUES(@Gender, @DateOfBirth, @FirstName, @LastName, @Street, @Suburb, @State, @PostCode, @HomePhone, @Mobile, @MedicareNumber, @Notes);
	END
-- =============================================================================
GO
-- ========================GET ALL PATIENTS=====================================
-- ***************************************************************************
CREATE PROCEDURE [dbo].[usp_GetAllPatients]
	

AS
BEGIN
	SELECT *
	FROM	PATIENT
END
-- ==============================================================================
GO
-- ========================GET ALL PRACTITIONERS=====================================
-- ***************************************************************************
CREATE PROCEDURE [dbo].[usp_GetAllPractitioners]
	

AS
BEGIN
	SELECT *
	FROM	PRACTITIONER
END
-- ==============================================================================
GO
-- =========================GET PATIENT APPOINTMENTS=============================
-- ******************************************************************************
CREATE PROCEDURE [dbo].[usp_GetPatientAppointments]
	@Patient_ID INTEGER
AS
BEGIN
	
	SELECT  p.Patient_ID,
			p.FirstName,
			p.LastName,
			a.AppointmentDate,
			a.AppointmentTime_Ref,
			pr.PractnrTypeName_Ref,
			pr.FirstName,
			pr.LastName
	FROM	PATIENT p,
			APPOINTMENT a,
			PRACTITIONER pr
	WHERE	p.Patient_ID = a.Patient_Ref 
	AND		a.Practitioner_Ref = pr.Practitioner_ID 
	AND		p.Patient_ID = @Patient_ID

END
-- ==============================================================================
GO
-- =====================CREATE APPOINTMENTS======================================
-- ******************************************************************************
CREATE PROCEDURE [dbo].[usp_CreateAppointment]
	@Practitioner_ID INTEGER,
	@AppointmentDate DATE,
	@AppointmentTime TIME,
	@Patient_ID		 INTEGER
	

AS
	SET NOCOUNT OFF;
	
	BEGIN

	INSERT INTO APPOINTMENT (
		Practitioner_Ref,
		AppointmentDate,
		AppointmentTime_Ref,
		Patient_Ref)
	VALUES (
		@Practitioner_ID,
		@AppointmentDate,
		@AppointmentTime,
		@Patient_ID)

	END
-- =========================================================================
GO
-- =================CANCEL APPOINTMENT======================================
-- *************************************************************************
CREATE PROCEDURE [dbo].[usp_CancelAppointment]
	@Practitioner_ID INTEGER,
	@AppointmentDate DATE,
	@AppointmentTime TIME,
	@Patient_ID		 INTEGER
AS
	SET NOCOUNT OFF;
BEGIN
	DELETE 
	FROM		APPOINTMENT
	WHERE		Practitioner_Ref = @Practitioner_ID
	AND			AppointmentDate = @AppointmentDate
	AND			AppointmentTime_Ref = @AppointmentTime
	AND			Patient_Ref = @Patient_ID;
END
-- =========================================================================
GO
-- ======================GET APPOINTMENTS BY PATIENT_ID=====================
-- *************************************************************************
CREATE PROCEDURE [dbo].[usp_GetAppointmentsByPatient_ID]
	@Patient_ID	INTEGER

AS

BEGIN
	SELECT *
	FROM	APPOINTMENT
	WHERE	Patient_Ref = @Patient_ID
END
GO
-- =================GET APPOINTMENT DETAILS BY PATIENT ID===================
-- *************************************************************************
--CREATE PROCEDURE [dbo].[ups_GetAppointmentDetailsByPatient_ID]
--	@Practitioner_ID	INTEGER,
--	@AppointmentDate	DATE,
--	@AppointmentTime	TIME,
--	@Patient_ID			INTEGER
--AS

--BEGIN
--	SELECT	a.AppointmentDate,
--			a.AppointmentTime_Ref,
--			pr.PractnrTypeName_Ref,
--			pr.FirstName,
--			pr.LastName
--	FROM	PATIENT p,
--			APPOINTMENT a,
--			PRACTITIONER pr
--	WHERE	p.Patient_ID = a.Patient_Ref
--	AND		a.Practitioner_Ref = pr.Practitioner_ID
--	AND		pr.Practitioner_ID = @Practitioner_ID
--	AND		a.AppointmentDate = @AppointmentDate
--	AND		a.AppointmentTime_Ref = @AppointmentTime
--	AND		p.Patient_ID = @Patient_ID 
--END
-- =========================================================================
GO
-- ===================GET PRACTITIONER======================================
-- **************************************************************************
CREATE PROCEDURE [dbo].[usp_GetPractitioner]
	@Practitioner_ID	INT

AS
BEGIN
	
	SELECT	*
	FROM	PRACTITIONER
	WHERE	Practitioner_ID = @Practitioner_ID

END
-- =========================================================================
GO
-- ====================GET APPOINTMENTS BY PRACTITIONER ID==================
-- *************************************************************************
CREATE PROCEDURE [dbo].[usp_GetAppointmentsByPractitioner_ID]
	@Practitioner_ID	INTEGER

AS

BEGIN
	SELECT	*
	FROM	
			APPOINTMENT a
			
	WHERE	a.Practitioner_Ref = @Practitioner_ID 
END
-- =========================================================================
GO
-- =================GET APPOINTMENT DETAILS BY PRACTITIONER ID==============
-- *************************************************************************
CREATE PROCEDURE [dbo].[usp_GetAppointDetailByPractitionerId]
	@Practitioner_ID	INTEGER
	
AS

BEGIN
	SELECT	p.Patient_ID,
			p.FirstName,
			p.LastName,
			a.AppointmentDate,
			a.AppointmentTime_Ref,
			pr.Practitioner_ID,
			pr.PractnrTypeName_Ref,
			pr.FirstName AS PracFirstName,
			pr.LastName AS PracLastName
	FROM	PATIENT p,
			APPOINTMENT a,
			PRACTITIONER pr
	WHERE	p.Patient_ID = a.Patient_Ref
	AND		a.Practitioner_Ref = pr.Practitioner_ID
	AND		pr.Practitioner_ID = @Practitioner_ID 
END
-- =========================================================================
GO
-- =========================UPDATE PRACTITIONER=============================
-- *************************************************************************
CREATE PROCEDURE [dbo].[usp_UpdatePractioner]
	@Practitioner_ID		INTEGER,
	@FirstName				NVARCHAR(50),
	@LastName				NVARCHAR(50),
	@Street					NVARCHAR(30),
	@Suburb					NVARCHAR(30),
	@State					NVARCHAR(3),
	@PostCode				NCHAR(4),
	@HomePhone				NVARCHAR(10),
	@Mobile					NVARCHAR(10),
	@RegistrationNumber		NVARCHAR(15),
	@PractnrTypeName_Ref	NVARCHAR(20),
	@Monday					BIT,
	@Tuesday				BIT,
	@Wednesday				BIT,
	@Thursday				BIT,
	@Friday					BIT,
	@Saturday				BIT,
	@Sunday					BIT
AS
	SET NOCOUNT OFF;
BEGIN
	UPDATE PRACTITIONER SET
			FirstName = @FirstName,
			LastName = @LastName,
			Street = @Street,
			Suburb = @Suburb,
			[State] = @State,
			PostCode = @PostCode,
			HomePhone = @HomePhone,
			Mobile = @Mobile,
			RegistrationNumber = @RegistrationNumber,
			PractnrTypeName_Ref = @PractnrTypeName_Ref,
			Monday = @Mobile,
			Tuesday = @Tuesday,
			Wednesday = @Wednesday,
			Thursday = @Thursday,
			Friday = @Friday,
			Saturday = @Saturday,
			Sunday = @Sunday
	WHERE	Practitioner_ID = @Practitioner_ID

END
-- =========================================================================
GO
-- =====================INSERT PRACTITIONER=================================
-- *************************************************************************
CREATE PROCEDURE [dbo].[usp_InsertPractitioner]
	@FirstName		NVARCHAR(50),
	@LastName		NVARCHAR(50),
	@Street			NVARCHAR(30),
	@Suburb			NVARCHAR(30),
	@State			NVARCHAR(3),
	@PostCode		NCHAR(4),
	@HomePhone		NVARCHAR(10),
	@Mobile			NVARCHAR(10),
	@RegistrationNumber	NVARCHAR(15),
	@PractnrTypeName_Ref	NVARCHAR(20),
	@Monday					BIT,
	@Tuesday				BIT,
	@Wednesday				BIT,
	@Thursday				BIT,
	@Friday					BIT,
	@Saturday				BIT,
	@Sunday					BIT

AS
BEGIN

	INSERT INTO PRACTITIONER (
		FirstName,
		LastName,
		Street,
		Suburb,
		[State],
		PostCode,
		HomePhone,
		Mobile,
		RegistrationNumber,
		PractnrTypeName_Ref,
		Monday,
		Tuesday,
		Wednesday,
		Thursday,
		Friday,
		Saturday,
		Sunday
	)
	VALUES (
		@FirstName,
		@LastName,
		@Street,
		@Suburb,
		@State,
		@PostCode,
		@HomePhone,
		@Mobile,
		@RegistrationNumber,
		@PractnrTypeName_Ref,
		@Monday,
		@Tuesday,
		@Wednesday,
		@Thursday,
		@Friday,
		@Saturday,
		@Sunday
	);

END
-- =========================================================================
GO
-- =======================DELETE PRACTITIONER======================================
-- ***************************************************************************
CREATE PROCEDURE [dbo].[usp_DeletePractitioner]
	@Practitioner_ID INTEGER
AS
	SET NOCOUNT OFF;
	BEGIN

	DELETE FROM PRACTITIONER WHERE Practitioner_ID = @Practitioner_ID;

	END
-- ===========================================================================
GO
-- ======================GET APPOINTMENT DETAILS BY PATIENT_ID================
-- ***************************************************************************
CREATE PROCEDURE [dbo].[usp_GetAppointDetailByPatientId]
	@Patient_Id INT

AS

BEGIN
	SELECT	pr.Practitioner_ID,
			pr.PractnrTypeName_Ref,
			pr.FirstName,
			pr.LastName,
			a.AppointmentDate,
			a.AppointmentTime_Ref,
			p.Patient_ID
	FROM	PRACTITIONER pr,
			APPOINTMENT a,
			PATIENT p
	WHERE	pr.Practitioner_ID = a.Practitioner_Ref
	AND		a.Patient_Ref = p.Patient_ID 
	AND		a.Patient_Ref = @Patient_Id
END
-- =========================================================================
GO
-- ========================GET TIMESLOTS====================================
-- *************************************************************************
CREATE PROCEDURE usp_GetTimeSlots 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM TIMESLOT

END
-- =========================================================================
GO
-- =======================usp_GetPractByDayAvail============================
-- ***************************************************************************
CREATE PROCEDURE [dbo].[usp_GetPractByDayAvail]
	@WeekDay  NVARCHAR(10) = NULL

	
AS
	DECLARE @SQL NVARCHAR(1000)

BEGIN

	SET @SQL = 'SELECT *
				FROM	PRACTITIONER
				WHERE ' + @WeekDay + ' = 1;'

	EXEC(@SQL)

END
-- =========================================================================
GO
-- =========================================================================
--				Time Slots ORIGINAL CODE.
-- =========================================================================
--DECLARE @t TABLE
--    (OfficeId INT, WeekdayId INT, StartTime TIME, EndTime TIME)

--INSERT INTO @t
--SELECT 1, 2, '14:30', '16:30'
--UNION ALL SELECT 1, 3, '16:00', '18:00'

--;WITH Tally (n) AS (
--    SELECT TOP 100 30*(ROW_NUMBER() OVER (ORDER BY (SELECT NULL))-1)
--    FROM sys.all_columns)
--SELECT OfficeID, WeekdayID
--    ,TSStart=DATEADD(minute, n, StartTime)
--    ,TSEnd=DATEADD(minute, n + 30, StartTime)
--    ,Timeslot=CONVERT(VARCHAR(100), DATEADD(minute, n, StartTime), 0) + ' - ' +
--        CONVERT(VARCHAR(100), DATEADD(minute, n + 30, StartTime), 0)
--FROM @t
--CROSS APPLY (
--    SELECT n 
--    FROM Tally 
--    WHERE n BETWEEN 0 AND DATEDIFF(minute, StartTime, DATEADD(minute, -30, EndTime))) a
--ORDER BY OfficeID, WeekdayID, TSStart
-- ===========================================================================