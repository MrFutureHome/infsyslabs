const router = require('express').Router();
const ctrl   = require('../controllers/recommendationController');
const { protect } = require('../middleware/auth');

router.get('/shopping-list/:classId', protect, ctrl.shoppingList);
router.get('/for-you',                protect, ctrl.recommendations);

module.exports = router;
