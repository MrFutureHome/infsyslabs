const Reg = require('../models/registrationModel');

exports.create = async (req, res, next) => {
  try {
    const result = await Reg.register(req.user.id, req.params.classId);
    if (!result) return res.status(409).json({ message: 'Already registered' });
    res.status(201).json(result);
  } catch (e) { next(e); }
};

exports.listMine = async (req, res, next) => {
  try { res.json(await Reg.getByUser(req.user.id)); }
  catch (e) { next(e); }
};

exports.remove = async (req, res, next) => {
  try {
    const deleted = await Reg.remove(req.user.id, req.params.id);
    if (!deleted) return res.status(404).json({ message: 'Registration not found' });
    res.json(deleted);
  } catch (e) { next(e); }
};
