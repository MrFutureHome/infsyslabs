HOST="${DATABASE_HOST:-postgres}"
PORT="${DATABASE_PORT:-5432}"
TIMEOUT=30  # максимальное время ожидания в секундах

echo "⏳ Waiting for database at $HOST:$PORT (timeout ${TIMEOUT}s)…"
while ! nc -z "$HOST" "$PORT" 2>/dev/null; do
  TIMEOUT=$((TIMEOUT - 1))
  if [ "$TIMEOUT" -le 0 ]; then
    echo "❌ ERROR: Timed out waiting for database $HOST:$PORT"
    exit 1
  fi
  sleep 1
done

echo "✅ Database is up — launching application"
# Передаём управление дальше: запускаем ту команду, которую передали в качестве аргументов
exec "$@"