const router = require('express').Router();
const ctrl   = require('../controllers/classController');
const { protect } = require('../middleware/auth');

router.route('/')
  .get(ctrl.getAll) // доступен всем
  .post(protect, ctrl.create);

router.route('/:id')
  .get(ctrl.getById)
  .put(protect, ctrl.update) // редактировать может только автор
  .delete(protect, ctrl.remove);

module.exports = router;
