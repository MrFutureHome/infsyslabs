const router = require('express').Router();
const ctrl   = require('../controllers/registrationController');
const { protect } = require('../middleware/auth');

router.post('/:classId', protect, ctrl.create);
router.get('/',          protect, ctrl.listMine);
router.delete('/:id',    protect, ctrl.remove);

module.exports = router;
