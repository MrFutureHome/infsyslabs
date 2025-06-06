require('dotenv').config();
const bcrypt = require('bcrypt');
const jwt    = require('jsonwebtoken');
const User   = require('../models/userModel');

const genToken = id => jwt.sign({ id }, process.env.JWT_SECRET, { expiresIn: '7d' });

exports.register = async (req, res, next) => {
  try {
    const { email, password } = req.body;
    if (!email || !password) throw new Error('Email & password required');
    if (await User.findByEmail(email)) throw new Error('User exists');

    const hash = await bcrypt.hash(password, 12);
    const user = await User.create(email, hash);
    res.status(201).json({ token: genToken(user.id), user });
  } catch (e) { next(e); }
};

exports.login = async (req, res, next) => {
  try {
    const { email, password } = req.body;
    const user = await User.findByEmail(email);
    if (!user || !(await bcrypt.compare(password, user.password)))
      return res.status(401).json({ message: 'Invalid credentials' });

    res.json({ token: genToken(user.id), user: { id: user.id, email: user.email, preferences: user.preferences } });
  } catch (e) { next(e); }
};

exports.me = async (req, res) => res.json(req.user);

exports.updatePrefs = async (req, res, next) => {
  try {
    res.json(await User.updatePreferences(req.user.id, req.body.preferences));
  } catch (e) { next(e); }
};
