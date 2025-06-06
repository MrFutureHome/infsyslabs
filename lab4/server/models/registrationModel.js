const db = require('../config/db');

const register = async (userId, classId) =>
  (await db.query(
    `INSERT INTO registrations (user_id, class_id)
     VALUES ($1,$2) ON CONFLICT (user_id,class_id) DO NOTHING
     RETURNING *`,
    [userId, classId]
  )).rows[0];

const getByUser = async userId =>
  (await db.query(
    `SELECT r.*, mc.title, mc.date FROM registrations r
     JOIN masterclasses mc ON mc.id = r.class_id
     WHERE r.user_id=$1 ORDER BY mc.date`,
    [userId]
  )).rows;

const remove = async (userId, regId) =>
  (await db.query('DELETE FROM registrations WHERE id=$1 AND user_id=$2 RETURNING *', [regId, userId])).rows[0];

module.exports = { register, getByUser, remove };
