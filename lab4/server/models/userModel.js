const db = require('../config/db');

const create = async (email, hash, preferences = {}) =>
  (await db.query(
    `INSERT INTO users (email, password, preferences)
     VALUES ($1,$2,$3) RETURNING id, email, preferences`,
    [email, hash, preferences]
  )).rows[0];

const findByEmail = async email =>
  (await db.query('SELECT * FROM users WHERE email=$1', [email])).rows[0];

const findById = async id =>
  (await db.query('SELECT id,email,preferences FROM users WHERE id=$1', [id])).rows[0];

const updatePreferences = async (id, prefs) =>
  (await db.query(
    'UPDATE users SET preferences=$2 WHERE id=$1 RETURNING id,email,preferences',
    [id, prefs]
  )).rows[0];

module.exports = { create, findByEmail, findById, updatePreferences };
