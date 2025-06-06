require('dotenv').config({ path: __dirname + '/.env' });

require('./config/db');

const express = require('express');
const cors = require('cors');
const path = require('path');

const app = express();

app.use(express.json());
app.use(cors());

app.get('/', (req, res) => {
  res.sendFile(path.join(__dirname, '..', 'client', 'index.html'));
});

app.get('/login', (req, res) => {
  res.sendFile(path.join(__dirname, '..', 'client', 'login.html'));
});

app.get('/register', (req, res) => {
  res.sendFile(path.join(__dirname, '..', 'client', 'register.html'));
});

app.get('/profile', (req, res) => {
  res.sendFile(path.join(__dirname, '..', 'client', 'profile.html'));
});

app.get('/masterclasses', (req, res) => {
  res.sendFile(path.join(__dirname, '..', 'client', 'masterclasses.html'));
});

app.get('/recipes', (req, res) => {
  res.sendFile(path.join(__dirname, '..', 'client', 'recipes.html'));
});

app.use(
  '/assets',
  express.static(path.join(__dirname, '..', 'client', 'assets'))
);

app.use('/api/classes',        require('./routes/classRoutes'));
app.use('/api/recipes',        require('./routes/recipeRoutes'));
app.use('/api/users',          require('./routes/userRoutes'));
app.use('/api/registrations',  require('./routes/registrationRoutes'));
app.use('/api/recommendations',require('./routes/recommendationRoutes'));

app.use((req, res) => {
  res.status(404).send('404 Not Found');
});

app.use(require('./middleware/errorHandler'));

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`API listening on http://localhost:${PORT}`));