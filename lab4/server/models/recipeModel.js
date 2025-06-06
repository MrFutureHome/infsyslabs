const db = require('../config/db');

const getAll = async () =>
  (await db.query(
    `SELECT r.*, u.email AS author_email
       FROM recipes r
       LEFT JOIN users u ON u.id = r.author_id
       ORDER BY title`
  )).rows;

const getById = async id =>
  (await db.query('SELECT * FROM recipes WHERE id=$1', [id])).rows[0];

const create = async data => {
  const { title, description, instructions, ingredients, author_id } = data;
  return (
    await db.query(
      `INSERT INTO recipes (title,description,instructions,ingredients,author_id)
       VALUES ($1,$2,$3::json,$4::json,$5) RETURNING *`,
      [
        title,
        description,
        JSON.stringify(instructions),
        JSON.stringify(ingredients),
        author_id
      ]
    )
  ).rows[0];
};

const update = async (id, authorId, data) => {
  const { title, description, instructions, ingredients } = data;
  return (
    await db.query(
      `UPDATE recipes
         SET title=$3, description=$4, instructions=$5, ingredients=$6
       WHERE id=$1 AND author_id=$2 RETURNING *`,
      [id, authorId, title, description, JSON.stringify(instructions), JSON.stringify(ingredients)]
    )
  ).rows[0];
};

const remove = async (id, authorId) =>
  (await db.query(
    'DELETE FROM recipes WHERE id=$1 AND author_id=$2 RETURNING *',
    [id, authorId]
  )).rows[0];

module.exports = { getAll, getById, create, update, remove };
