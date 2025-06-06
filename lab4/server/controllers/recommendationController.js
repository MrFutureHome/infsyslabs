const Rec = require('../models/recommendationModel');

exports.shoppingList = async (req, res, next) => {
  try { res.json(await Rec.getShoppingListForClass(req.params.classId)); }
  catch (e) { next(e); }
};

exports.recommendations = async (req, res, next) => {
  try { res.json(await Rec.getRecommendedClasses(req.user.id)); }
  catch (e) { next(e); }
};
