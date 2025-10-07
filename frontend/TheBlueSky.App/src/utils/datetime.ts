export function formatDate(value: string | Date, locale = 'en-IN') {
  const d = typeof value === 'string' ? new Date(value) : value;
  return d.toLocaleDateString(locale, { day: '2-digit', month: 'short', year: 'numeric' });
}

export function formatTime(value: string | Date, locale = 'en-IN') {
  const d = typeof value === 'string' ? new Date(value) : value;
  return d.toLocaleTimeString(locale, { hour: '2-digit', minute: '2-digit' });
}