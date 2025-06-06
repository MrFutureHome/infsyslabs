const router = require('express').Router();
const ctrl   = require('../controllers/userController');
const { protect } = require('../middleware/auth');

router.post('/register', ctrl.register);
router.post('/login',    ctrl.login);

router.get('/me', protect, ctrl.me);
router.put('/me/preferences', protect, ctrl.updatePrefs);

module.exports = router;
