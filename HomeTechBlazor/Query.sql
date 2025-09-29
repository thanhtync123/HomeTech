-- Database: hometech_db
CREATE DATABASE hometech_db;
USE hometech_db;

-- ==========================================
-- Table: users
-- ==========================================
CREATE TABLE users (
    id INT NOT NULL AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    phone VARCHAR(20) NOT NULL UNIQUE,
    password VARCHAR(100) NOT NULL,
    address VARCHAR(200),
    role ENUM('admin','customer','technical') DEFAULT 'customer',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (id)
);

-- ==========================================
-- Table: services
-- ==========================================
CREATE TABLE services (
    id INT NOT NULL AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    description TEXT,
    price INT NOT NULL,
    unit VARCHAR(50),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (id)
);

-- ==========================================
-- Table: orders
-- ==========================================
CREATE TABLE orders (
    id INT NOT NULL AUTO_INCREMENT,
    customer_id INT NOT NULL,
    service_id INT NOT NULL,
    technician_id INT,
    schedule_time DATETIME NOT NULL,
    status ENUM('pending','completed','cancelled') DEFAULT 'pending',
    total_price INT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    FOREIGN KEY (customer_id) REFERENCES users(id),
    FOREIGN KEY (service_id) REFERENCES services(id),
    FOREIGN KEY (technician_id) REFERENCES users(id)
);

-- ==========================================
-- Table: orderequipments
-- ==========================================
CREATE TABLE orderequipments (
    id INT NOT NULL AUTO_INCREMENT,
    order_id INT NOT NULL,
    name VARCHAR(100) NOT NULL,
    quantity INT DEFAULT 1,
    price INT DEFAULT 0,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    FOREIGN KEY (order_id) REFERENCES orders(id)
);

-- ==========================================
-- Table: reviews
-- ==========================================
CREATE TABLE reviews (
    id INT NOT NULL AUTO_INCREMENT,
    order_id INT NOT NULL,
    customer_id INT NOT NULL,
    rating TINYINT CHECK (rating BETWEEN 1 AND 5),
    comment TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    FOREIGN KEY (order_id) REFERENCES orders(id),
    FOREIGN KEY (customer_id) REFERENCES users(id)
);
-- Alter =============================================================================

-- ====================================================================================

SELECT 
    o.id,
    c.name AS customer_name,
    t.name AS technician_name,
    s.name AS service_name,
    o.schedule_time,
    o.status,
    o.total_price,
    o.created_at,
    o.updated_at
FROM orders o
JOIN services s ON o.service_id = s.id
JOIN users c ON o.customer_id = c.id
LEFT JOIN users t ON o.technician_id = t.id;

INSERT INTO orders (customer_id, service_id, technician_id, schedule_time, status, total_price)
VALUES
(1, 1, 2, '2025-09-25 09:00:00', 'pending', 250000),
(1, 2, 2, '2025-09-26 14:30:00', 'completed', 150000),
(1, 1, NULL, '2025-09-27 10:00:00', 'cancelled', 250000);

  SELECT 
    o.id,
    c.name AS customer_name,
    t.name AS technician_name,
    s.name AS service_name,
    DATE_FORMAT(o.schedule_time, '%d%m%Y %H:%i') AS schedule_time,
    o.status,
    o.total_price,
    DATE_FORMAT(o.created_at, '%d%m%Y %H:%i') AS created_at,
    DATE_FORMAT(o.updated_at, '%d%m%Y %H:%i') AS updated_at
FROM orders o
JOIN services s ON o.service_id = s.id
JOIN users c ON o.customer_id = c.id
LEFT JOIN users t ON o.technician_id = t.id;


INSERT INTO users (name, phone, password, address, role) VALUES
('Nguyen Van D', '0900000004', '123456', 'Hai Phong', 'customer')

DELETE FROM users where id = 50
            UPDATE users
            SET
                name = 'Pham Van Renbnnn',
                phone = '0900000018',
                password = '123456',
                address = 'Gia Lai',
                role = 'technical'
            WHERE id = 43

            UPDATE users
            SET
                name = 'Pham Van Renbnnn123123213',
                phone = '0900000018',
                password = '123456',
                address = 'Gia Lai',
                role = 'technical',
            WHERE id = 43;
select count(*) from users where phone = '0914161844' and password = '123abc456'

ALTER TABLE orderequipments
DROP COLUMN `name`,
DROP COLUMN `unit`,
DROP COLUMN `quantity`;
CREATE TABLE equipments
(
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    unit VARCHAR(100),
    price DECIMAL(18,2),
    quantity INT,
    description TEXT
) CHARACTER SET utf8mb4;

INSERT INTO equipments (name, unit, price, quantity, description)
VALUES 
(N'Laptop Dell', N'cái', 15000000, 10, N'Máy tính xách tay Dell Inspiron'),
(N'Màn hình Samsung', N'cái', 3500000, 20, N'Màn hình LCD 24 inch'),
(N'Bàn phím Logitech', N'cái', 450000, 50, N'Bàn phím không dây Logitech K380'),
(N'Ghế văn phòng', N'cái', 1200000, 15, N'Ghế xoay nhân viên'),
(N'Máy in HP', N'cái', 2500000, 5, N'Máy in laser HP 107w');
select * from equipments

Select * from orderequipments
                Select id, name, unit, price, quantity, description
                from equipments
                where name LIKE '%%' or id LIKE '%%'
