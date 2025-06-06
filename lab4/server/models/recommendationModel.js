const db = require('../config/db');

const getShoppingListForClass = async classId => {
  const res = await db.query('SELECT ingredients FROM masterclasses WHERE id=$1', [classId]);
  return res.rows[0]?.ingredients ?? [];
};

const getRecommendedClasses = async userId =>
  (await db.query(
    `SELECT mc.*
       FROM masterclasses mc
       WHERE mc.id NOT IN (SELECT class_id FROM registrations WHERE user_id=$1)
       ORDER BY mc.date
       LIMIT 3`,
    [userId]
  )).rows;

module.exports = { getShoppingListForClass, getRecommendedClasses };
