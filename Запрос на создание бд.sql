CREATE TABLE Roles (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Users (
    id INT IDENTITY(1,1) PRIMARY KEY,
    role_id INT NOT NULL REFERENCES Roles(id),
    full_name VARCHAR(255) NOT NULL,
    login VARCHAR(255) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL
);

CREATE TABLE PickupPoints (
    id INT PRIMARY KEY,
    address TEXT NOT NULL
);

CREATE TABLE Suppliers (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE Manufacturers (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE Categories (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE Units (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Products (
    article VARCHAR(20) PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    unit_id INT NOT NULL REFERENCES Units(id),
    price DECIMAL(10,2) NOT NULL CHECK (price >= 0),
    supplier_id INT NOT NULL REFERENCES Suppliers(id),
    manufacturer_id INT NOT NULL REFERENCES Manufacturers(id),
    category_id INT NOT NULL REFERENCES Categories(id),
    discount DECIMAL(5,2) NOT NULL DEFAULT 0 CHECK (discount >= 0),
    stock_quantity INT NOT NULL CHECK (stock_quantity >= 0),
    description TEXT,
    photo_path VARCHAR(255)
);

CREATE TABLE OrderStatuses (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Orders (
    id INT PRIMARY KEY,
    order_date DATE NOT NULL,
    delivery_date DATE NOT NULL,
    pickup_point_id INT NOT NULL REFERENCES PickupPoints(id),
    user_id INT NOT NULL REFERENCES Users(id),
    pickup_code INT NOT NULL UNIQUE,
    status_id INT NOT NULL REFERENCES OrderStatuses(id)
);

CREATE TABLE OrderItems (
    order_id INT NOT NULL REFERENCES Orders(id),
    product_article VARCHAR(20) NOT NULL REFERENCES Products(article),
    quantity INT NOT NULL CHECK (quantity > 0),
    PRIMARY KEY (order_id, product_article)
);

INSERT INTO Roles (name) VALUES ('Администратор'), ('Менеджер'), ('Авторизированный клиент');

INSERT INTO OrderStatuses (name) VALUES ('Завершен'), ('Новый');

INSERT INTO Units (name) VALUES ('шт.');

INSERT INTO Suppliers (name) VALUES ('Kari'), ('Обувь для вас');

INSERT INTO Manufacturers (name) VALUES ('Kari'), ('Marco Tozzi'), ('Рос'), ('Rieker'), ('Alessio Nesca'), ('CROSBY');

INSERT INTO Categories (name) VALUES ('Женская обувь'), ('Мужская обувь');

INSERT INTO Users (role_id, full_name, login, password) VALUES
    ((SELECT id FROM Roles WHERE name='Администратор'), 'Никифорова Весения Николаевна', '94d5ous@gmail.com', 'uzWC67'),
    ((SELECT id FROM Roles WHERE name='Администратор'), 'Сазонов Руслан Германович', 'uth4iz@mail.com', '2L6KZG'),
    ((SELECT id FROM Roles WHERE name='Администратор'), 'Одинцов Серафим Артёмович', 'yzls62@outlook.com', 'JlFRCZ'),
    ((SELECT id FROM Roles WHERE name='Менеджер'), 'Степанов Михаил Артёмович', '1diph5e@tutanota.com', '8ntwUp'),
    ((SELECT id FROM Roles WHERE name='Менеджер'), 'Ворсин Петр Евгеньевич', 'tjde7c@yahoo.com', 'YOyhfR'),
    ((SELECT id FROM Roles WHERE name='Менеджер'), 'Старикова Елена Павловна', 'wpmrc3do@tutanota.com', 'RSbvHv'),
    ((SELECT id FROM Roles WHERE name='Авторизированный клиент'), 'Михайлюк Анна Вячеславовна', '5d4zbu@tutanota.com', 'rwVDh9'),
    ((SELECT id FROM Roles WHERE name='Авторизированный клиент'), 'Ситдикова Елена Анатольевна', 'ptec8ym@yahoo.com', 'LdNyos'),
    ((SELECT id FROM Roles WHERE name='Авторизированный клиент'), 'Ворсин Петр Евгеньевич', '1qz4kw@mail.com', 'gynQMT'),
    ((SELECT id FROM Roles WHERE name='Авторизированный клиент'), 'Старикова Елена Павловна', '4np6se@mail.com', 'AtnDjr');

INSERT INTO PickupPoints (id, address) VALUES
    (1,  '420151, г. Лесной, ул. Вишневая, 32'),
    (2,  '125061, г. Лесной, ул. Подгорная, 8'),
    (3,  '630370, г. Лесной, ул. Шоссейная, 24'),
    (4,  '400562, г. Лесной, ул. Зеленая, 32'),
    (5,  '614510, г. Лесной, ул. Маяковского, 47'),
    (6,  '410542, г. Лесной, ул. Светлая, 46'),
    (7,  '620839, г. Лесной, ул. Цветочная, 8'),
    (8,  '443890, г. Лесной, ул. Коммунистическая, 1'),
    (9,  '603379, г. Лесной, ул. Спортивная, 46'),
    (10, '603721, г. Лесной, ул. Гоголя, 41'),
    (11, '410172, г. Лесной, ул. Северная, 13'),
    (12, '614611, г. Лесной, ул. Молодежная, 50'),
    (13, '454311, г.Лесной, ул. Новая, 19'),
    (14, '660007, г.Лесной, ул. Октябрьская, 19'),
    (15, '603036, г. Лесной, ул. Садовая, 4'),
    (16, '394060, г.Лесной, ул. Фрунзе, 43'),
    (17, '410661, г. Лесной, ул. Школьная, 50'),
    (18, '625590, г. Лесной, ул. Коммунистическая, 20'),
    (19, '625683, г. Лесной, ул. 8 Марта'),
    (20, '450983, г.Лесной, ул. Комсомольская, 26'),
    (21, '394782, г. Лесной, ул. Чехова, 3'),
    (22, '603002, г. Лесной, ул. Дзержинского, 28'),
    (23, '450558, г. Лесной, ул. Набережная, 30'),
    (24, '344288, г. Лесной, ул. Чехова, 1'),
    (25, '614164, г.Лесной,  ул. Степная, 30'),
    (26, '394242, г. Лесной, ул. Коммунистическая, 43'),
    (27, '660540, г. Лесной, ул. Солнечная, 25'),
    (28, '125837, г. Лесной, ул. Шоссейная, 40'),
    (29, '125703, г. Лесной, ул. Партизанская, 49'),
    (30, '625283, г. Лесной, ул. Победы, 46'),
    (31, '614753, г. Лесной, ул. Полевая, 35'),
    (32, '426030, г. Лесной, ул. Маяковского, 44'),
    (33, '450375, г. Лесной ул. Клубная, 44'),
    (34, '625560, г. Лесной, ул. Некрасова, 12'),
    (35, '630201, г. Лесной, ул. Комсомольская, 17'),
    (36, '190949, г. Лесной, ул. Мичурина, 26');

INSERT INTO Products (article, name, unit_id, price, supplier_id, manufacturer_id, category_id, discount, stock_quantity, description, photo_path) VALUES
    ('А112Т4', 'Ботинки', (SELECT id FROM Units WHERE name='шт.'), 4990.00,
        (SELECT id FROM Suppliers WHERE name='Kari'),
        (SELECT id FROM Manufacturers WHERE name='Kari'),
        (SELECT id FROM Categories WHERE name='Женская обувь'), 3, 6, 'Женские Ботинки демисезонные kari', '1.jpg'),
    ('F635R4', 'Ботинки', (SELECT id FROM Units WHERE name='шт.'), 3244.00,
        (SELECT id FROM Suppliers WHERE name='Обувь для вас'),
        (SELECT id FROM Manufacturers WHERE name='Marco Tozzi'),
        (SELECT id FROM Categories WHERE name='Женская обувь'), 2, 13, 'Ботинки Marco Tozzi женские демисезонные, размер 39, цвет бежевый', '2.jpg'),
    ('H782T5', 'Туфли', (SELECT id FROM Units WHERE name='шт.'), 4499.00,
        (SELECT id FROM Suppliers WHERE name='Kari'),
        (SELECT id FROM Manufacturers WHERE name='Kari'),
        (SELECT id FROM Categories WHERE name='Мужская обувь'), 4, 5, 'Туфли kari мужские классика MYZ21AW-450A, размер 43, цвет: черный', '3.jpg'),
    ('G783F5', 'Ботинки', (SELECT id FROM Units WHERE name='шт.'), 5900.00,
        (SELECT id FROM Suppliers WHERE name='Kari'),
        (SELECT id FROM Manufacturers WHERE name='Рос'),
        (SELECT id FROM Categories WHERE name='Мужская обувь'), 2, 8, 'Мужские ботинки Рос-Обувь кожаные с натуральным мехом', '4.jpg'),
    ('J384T6', 'Ботинки', (SELECT id FROM Units WHERE name='шт.'), 3800.00,
        (SELECT id FROM Suppliers WHERE name='Обувь для вас'),
        (SELECT id FROM Manufacturers WHERE name='Rieker'),
        (SELECT id FROM Categories WHERE name='Мужская обувь'), 2, 16, 'B3430/14 Полуботинки мужские Rieker', '5.jpg'),
    ('D572U8', 'Кроссовки', (SELECT id FROM Units WHERE name='шт.'), 4100.00,
        (SELECT id FROM Suppliers WHERE name='Обувь для вас'),
        (SELECT id FROM Manufacturers WHERE name='Рос'),
        (SELECT id FROM Categories WHERE name='Мужская обувь'), 3, 6, '129615-4 Кроссовки мужские', '6.jpg'),
    ('F572H7', 'Туфли', (SELECT id FROM Units WHERE name='шт.'), 2700.00,
        (SELECT id FROM Suppliers WHERE name='Kari'),
        (SELECT id FROM Manufacturers WHERE name='Marco Tozzi'),
        (SELECT id FROM Categories WHERE name='Женская обувь'), 2, 14, 'Туфли Marco Tozzi женские летние, размер 39, цвет черный', '7.jpg'),
    ('D329H3', 'Полуботинки', (SELECT id FROM Units WHERE name='шт.'), 1890.00,
        (SELECT id FROM Suppliers WHERE name='Обувь для вас'),
        (SELECT id FROM Manufacturers WHERE name='Alessio Nesca'),
        (SELECT id FROM Categories WHERE name='Женская обувь'), 4, 4, 'Полуботинки Alessio Nesca женские 3-30797-47, размер 37, цвет: бордовый', '8.jpg'),
    ('B320R5', 'Туфли', (SELECT id FROM Units WHERE name='шт.'), 4300.00,
        (SELECT id FROM Suppliers WHERE name='Kari'),
        (SELECT id FROM Manufacturers WHERE name='Rieker'),
        (SELECT id FROM Categories WHERE name='Женская обувь'), 2, 6, 'Туфли Rieker женские демисезонные, размер 41, цвет коричневый', '9.jpg'),
    ('G432E4', 'Туфли', (SELECT id FROM Units WHERE name='шт.'), 2800.00,
        (SELECT id FROM Suppliers WHERE name='Kari'),
        (SELECT id FROM Manufacturers WHERE name='Kari'),
        (SELECT id FROM Categories WHERE name='Женская обувь'), 3, 15, 'Туфли kari женские TR-YR-413017, размер 37, цвет: черный', '10.jpg'),
    ('S213E3', 'Полуботинки', (SELECT id FROM Units WHERE name='шт.'), 2156.00,
        (SELECT id FROM Suppliers WHERE name='Обувь для вас'),
        (SELECT id FROM Manufacturers WHERE name='CROSBY'),
        (SELECT id FROM Categories WHERE name='Мужская обувь'), 3, 6, '407700/01-01 Полуботинки мужские CROSBY', NULL),
    ('E482R4', 'Полуботинки', (SELECT id FROM Units WHERE name='шт.'), 1800.00,
        (SELECT id FROM Suppliers WHERE name='Kari'),
        (SELECT id FROM Manufacturers WHERE name='Kari'),
        (SELECT id FROM Categories WHERE name='Женская обувь'), 2, 14, 'Полуботинки kari женские MYZ20S-149, размер 41, цвет: черный', NULL),
    ('S634B5', 'Кеды', (SELECT id FROM Units WHERE name='шт.'), 5500.00,
        (SELECT id FROM Suppliers WHERE name='Обувь для вас'),
        (SELECT id FROM Manufacturers WHERE name='CROSBY'),
        (SELECT id FROM Categories WHERE name='Мужская обувь'), 3, 0, 'Кеды Caprice мужские демисезонные, размер 42, цвет черный', NULL),
    ('K345R4', 'Полуботинки', (SELECT id FROM Units WHERE name='шт.'), 2100.00,
        (SELECT id FROM Suppliers WHERE name='Обувь для вас'),
        (SELECT id FROM Manufacturers WHERE name='CROSBY'),
        (SELECT id FROM Categories WHERE name='Мужская обувь'), 2, 3, '407700/01-02 Полуботинки мужские CROSBY', NULL),
    ('O754F4', 'Туфли', (SELECT id FROM Units WHERE name='шт.'), 5400.00,
        (SELECT id FROM Suppliers WHERE name='Обувь для вас'),
        (SELECT id FROM Manufacturers WHERE name='Rieker'),
        (SELECT id FROM Categories WHERE name='Женская обувь'), 4, 18, 'Туфли женские демисезонные Rieker артикул 55073-68/37', NULL),
    ('G531F4', 'Ботинки', (SELECT id FROM Units WHERE name='шт.'), 6600.00,
        (SELECT id FROM Suppliers WHERE name='Kari'),
        (SELECT id FROM Manufacturers WHERE name='Kari'),
        (SELECT id FROM Categories WHERE name='Женская обувь'), 12, 9, 'Ботинки женские зимние ROMER арт. 893167-01 Черный', NULL),
    ('J542F5', 'Тапочки', (SELECT id FROM Units WHERE name='шт.'), 500.00,
        (SELECT id FROM Suppliers WHERE name='Kari'),
        (SELECT id FROM Manufacturers WHERE name='Kari'),
        (SELECT id FROM Categories WHERE name='Мужская обувь'), 13, 0, 'Тапочки мужские Арт.70701-55-67син р.41', NULL),
    ('B431R5', 'Ботинки', (SELECT id FROM Units WHERE name='шт.'), 2700.00,
        (SELECT id FROM Suppliers WHERE name='Обувь для вас'),
        (SELECT id FROM Manufacturers WHERE name='Rieker'),
        (SELECT id FROM Categories WHERE name='Мужская обувь'), 2, 5, 'Мужские кожаные ботинки/мужские ботинки', NULL),
    ('P764G4', 'Туфли', (SELECT id FROM Units WHERE name='шт.'), 6800.00,
        (SELECT id FROM Suppliers WHERE name='Kari'),
        (SELECT id FROM Manufacturers WHERE name='CROSBY'),
        (SELECT id FROM Categories WHERE name='Женская обувь'), 15, 15, 'Туфли женские, ARGO, размер 38', NULL),
    ('C436G5', 'Ботинки', (SELECT id FROM Units WHERE name='шт.'), 10200.00,
        (SELECT id FROM Suppliers WHERE name='Kari'),
        (SELECT id FROM Manufacturers WHERE name='Alessio Nesca'),
        (SELECT id FROM Categories WHERE name='Женская обувь'), 15, 9, 'Ботинки женские, ARGO, размер 40', NULL),
    ('F427R5', 'Ботинки', (SELECT id FROM Units WHERE name='шт.'), 11800.00,
        (SELECT id FROM Suppliers WHERE name='Обувь для вас'),
        (SELECT id FROM Manufacturers WHERE name='Rieker'),
        (SELECT id FROM Categories WHERE name='Женская обувь'), 15, 11, 'Ботинки на молнии с декоративной пряжкой FRAU', NULL),
    ('N457T5', 'Полуботинки', (SELECT id FROM Units WHERE name='шт.'), 4600.00,
        (SELECT id FROM Suppliers WHERE name='Kari'),
        (SELECT id FROM Manufacturers WHERE name='CROSBY'),
        (SELECT id FROM Categories WHERE name='Женская обувь'), 3, 13, 'Полуботинки Ботинки черные зимние, мех', NULL),
    ('D364R4', 'Туфли', (SELECT id FROM Units WHERE name='шт.'), 12400.00,
        (SELECT id FROM Suppliers WHERE name='Kari'),
        (SELECT id FROM Manufacturers WHERE name='Kari'),
        (SELECT id FROM Categories WHERE name='Женская обувь'), 16, 5, 'Туфли Luiza Belly женские Kate-lazo черные из натуральной замши', NULL),
    ('S326R5', 'Тапочки', (SELECT id FROM Units WHERE name='шт.'), 9900.00,
        (SELECT id FROM Suppliers WHERE name='Обувь для вас'),
        (SELECT id FROM Manufacturers WHERE name='CROSBY'),
        (SELECT id FROM Categories WHERE name='Мужская обувь'), 17, 15, 'Мужские кожаные тапочки "Профиль С.Дали"', NULL),
    ('L754R4', 'Полуботинки', (SELECT id FROM Units WHERE name='шт.'), 1700.00,
        (SELECT id FROM Suppliers WHERE name='Kari'),
        (SELECT id FROM Manufacturers WHERE name='Kari'),
        (SELECT id FROM Categories WHERE name='Женская обувь'), 2, 7, 'Полуботинки kari женские WB2020SS-26, размер 38, цвет: черный', NULL),
    ('M542T5', 'Кроссовки', (SELECT id FROM Units WHERE name='шт.'), 2800.00,
        (SELECT id FROM Suppliers WHERE name='Обувь для вас'),
        (SELECT id FROM Manufacturers WHERE name='Rieker'),
        (SELECT id FROM Categories WHERE name='Мужская обувь'), 18, 3, 'Кроссовки мужские TOFA', NULL),
    ('D268G5', 'Туфли', (SELECT id FROM Units WHERE name='шт.'), 4399.00,
        (SELECT id FROM Suppliers WHERE name='Обувь для вас'),
        (SELECT id FROM Manufacturers WHERE name='Rieker'),
        (SELECT id FROM Categories WHERE name='Женская обувь'), 3, 12, 'Туфли Rieker женские демисезонные, размер 36, цвет коричневый', NULL),
    ('T324F5', 'Сапоги', (SELECT id FROM Units WHERE name='шт.'), 4699.00,
        (SELECT id FROM Suppliers WHERE name='Kari'),
        (SELECT id FROM Manufacturers WHERE name='CROSBY'),
        (SELECT id FROM Categories WHERE name='Женская обувь'), 2, 5, 'Сапоги замша Цвет: синий', NULL),
    ('K358H6', 'Тапочки', (SELECT id FROM Units WHERE name='шт.'), 599.00,
        (SELECT id FROM Suppliers WHERE name='Kari'),
        (SELECT id FROM Manufacturers WHERE name='Rieker'),
        (SELECT id FROM Categories WHERE name='Мужская обувь'), 20, 2, 'Тапочки мужские син р.41', NULL),
    ('H535R5', 'Ботинки', (SELECT id FROM Units WHERE name='шт.'), 2300.00,
        (SELECT id FROM Suppliers WHERE name='Обувь для вас'),
        (SELECT id FROM Manufacturers WHERE name='Rieker'),
        (SELECT id FROM Categories WHERE name='Женская обувь'), 2, 7, 'Женские Ботинки демисезонные', NULL);

INSERT INTO Orders (id, order_date, delivery_date, pickup_point_id, user_id, pickup_code, status_id) VALUES
    (1, '2025-02-27', '2025-04-20', 1,
        (SELECT id FROM Users WHERE full_name='Степанов Михаил Артёмович'),
        901, (SELECT id FROM OrderStatuses WHERE name='Завершен')),
    (2, '2022-09-28', '2025-04-21', 11,
        (SELECT id FROM Users WHERE full_name='Никифорова Весения Николаевна'),
        902, (SELECT id FROM OrderStatuses WHERE name='Завершен')),
    (3, '2025-03-21', '2025-04-22', 2,
        (SELECT id FROM Users WHERE full_name='Сазонов Руслан Германович'),
        903, (SELECT id FROM OrderStatuses WHERE name='Завершен')),
    (4, '2025-02-20', '2025-04-23', 11,
        (SELECT id FROM Users WHERE full_name='Одинцов Серафим Артёмович'),
        904, (SELECT id FROM OrderStatuses WHERE name='Завершен')),
    (5, '2025-03-17', '2025-04-24', 2,
        (SELECT id FROM Users WHERE full_name='Степанов Михаил Артёмович'),
        905, (SELECT id FROM OrderStatuses WHERE name='Завершен')),
    (6, '2025-03-01', '2025-04-25', 15,
        (SELECT id FROM Users WHERE full_name='Никифорова Весения Николаевна'),
        906, (SELECT id FROM OrderStatuses WHERE name='Завершен')),
    (7, '2025-03-02', '2025-04-26', 3,
        (SELECT id FROM Users WHERE full_name='Сазонов Руслан Германович'),
        907, (SELECT id FROM OrderStatuses WHERE name='Завершен')),
    (8, '2025-03-31', '2025-04-27', 19,
        (SELECT id FROM Users WHERE full_name='Одинцов Серафим Артёмович'),
        908, (SELECT id FROM OrderStatuses WHERE name='Новый')),
    (9, '2025-04-02', '2025-04-28', 5,
        (SELECT id FROM Users WHERE full_name='Степанов Михаил Артёмович'),
        909, (SELECT id FROM OrderStatuses WHERE name='Новый')),
    (10, '2025-04-03', '2025-04-29', 19,
        (SELECT id FROM Users WHERE full_name='Степанов Михаил Артёмович'),
        910, (SELECT id FROM OrderStatuses WHERE name='Новый'));

INSERT INTO OrderItems (order_id, product_article, quantity) VALUES
    (1, 'А112Т4', 2),
    (1, 'F635R4', 2),
    (2, 'H782T5', 1),
    (2, 'G783F5', 1),
    (3, 'J384T6', 10),
    (3, 'D572U8', 10),
    (4, 'F572H7', 5),
    (4, 'D329H3', 4),
    (5, 'А112Т4', 2),
    (5, 'F635R4', 2),
    (6, 'H782T5', 1),
    (6, 'G783F5', 1),
    (7, 'J384T6', 10),
    (7, 'D572U8', 10),
    (8, 'F572H7', 5),
    (8, 'D329H3', 4),
    (9, 'B320R5', 5),
    (9, 'G432E4', 1),
    (10, 'S213E3', 5),
    (10, 'E482R4', 5);