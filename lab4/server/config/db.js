require('dotenv').config({ path: __dirname + '/../.env' });
const { Client } = require('pg');

const client = new Client({
  user:     process.env.DB_USER,
  host:     process.env.DB_HOST,
  database: process.env.DB_NAME,
  password: process.env.DB_PASSWORD,
  port:     process.env.DB_PORT
});

client.connect()
  .then(()=> console.log('db connected'))
  .catch(err=>{
    console.error('db connection error:', err.message);
    process.exit(1);
  });

module.exports = client;