const router = require('express').Router();
const ctrl   = require('../controllers/recipeController');
const { protect } = require('../middleware/auth');

router.route('/')
  .get(ctrl.getAll)
  .post(protect, ctrl.create);           // создать рецепт могут авторизованные

router.route('/:id')
  .get(ctrl.getById)
  .put(protect, ctrl.update)
  .delete(protect, ctrl.remove);

module.exports = router;
