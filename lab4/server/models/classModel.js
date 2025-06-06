const db = require('../config/db');

/*** MASTERCLASSES **********************************************************/

const getAll = async () =>
  (await db.query(
    `SELECT mc.*, u.email AS author_email
       FROM masterclasses mc
       LEFT JOIN users u ON u.id = mc.author_id
       ORDER BY date`
  )).rows;

const getById = async id =>
  (await db.query('SELECT * FROM masterclasses WHERE id=$1', [id])).rows[0];

const create = async data => {
  const { title, description, video_url, date, chef, ingredients, author_id } = data;
  return (
    await db.query(
      `INSERT INTO masterclasses
         (title,description,video_url,date,chef,ingredients,author_id)
       VALUES ($1,$2,$3,$4,$5,$6,$7)
       RETURNING *`,
      [title, description, video_url, date, chef, ingredients, author_id]
    )
  ).rows[0];
};

const update = async (id, authorId, data) => {
  const { title, description, video_url, date, chef, ingredients } = data;
  return (
    await db.query(
      `UPDATE masterclasses
         SET title=$3, description=$4, video_url=$5, date=$6, chef=$7, ingredients=$8
       WHERE id=$1 AND author_id=$2
       RETURNING *`,
      [id, authorId, title, description, video_url, date, chef, ingredients]
    )
  ).rows[0];
};

const remove = async (id, authorId) =>
  (await db.query(
    'DELETE FROM masterclasses WHERE id=$1 AND author_id=$2 RETURNING *',
    [id, authorId]
  )).rows[0];

module.exports = { getAll, getById, create, update, remove };
