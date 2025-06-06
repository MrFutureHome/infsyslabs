const Recipe = require('../models/recipeModel');

exports.getAll  = async (_ , res , next) => { try { res.json(await Recipe.getAll()); } catch(e){ next(e);} };
exports.getById = async (req, res, next) => { try { res.json(await Recipe.getById(req.params.id)); } catch(e){ next(e);} };

exports.create  = async (req, res, next) => {
  try { res.status(201).json(await Recipe.create({ ...req.body, author_id: req.user.id })); }
  catch(e){ next(e);}
};

exports.update  = async (req, res, next) => {
  try {
    const updated = await Recipe.update(req.params.id, req.user.id, req.body);
    if (!updated) return res.status(403).json({ message:'Not your recipe' });
    res.json(updated);
  } catch(e){ next(e);}
};

exports.remove  = async (req, res, next) => {
  try {
    const deleted = await Recipe.remove(req.params.id, req.user.id);
    if (!deleted) return res.status(403).json({ message:'Not your recipe' });
    res.json(deleted);
  } catch(e){ next(e);}
};

