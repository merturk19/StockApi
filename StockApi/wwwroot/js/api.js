// Global API utilities for the SPA
// Point to API running in Docker on localhost:5000
window.API_BASE = 'http://localhost:5000';
window.API = {
  routes: {
    list: () => `${window.API_BASE}/stockapi/StockItem/GetAll`,
    get: (id) => `${window.API_BASE}/stockapi/StockItem/Get?id=${encodeURIComponent(id)}`,
    add: () => `${window.API_BASE}/stockapi/StockItem/AddStockItem`,
    update: () => `${window.API_BASE}/stockapi/StockItem/UpdateStockItem`,
    delete: (id) => `${window.API_BASE}/stockapi/StockItem/DeleteStockItem?id=${encodeURIComponent(id)}`,
  },
  async http(method, url, body) {
    const res = await fetch(url, {
      method,
      headers: body ? { 'Content-Type': 'application/json' } : undefined,
      body: body ? JSON.stringify(body) : undefined,
    });
    if (!res.ok) {
      const text = await res.text();
      throw new Error(text || `HTTP ${res.status}`);
    }
    const contentType = res.headers.get('content-type') || '';
    if (contentType.includes('application/json')) return res.json();
    return res.text();
  },
  getAllItems() { return window.API.http('GET', window.API.routes.list()); },
  addItem(name) { return window.API.http('POST', window.API.routes.add(), { name }); },
  updateItem(id, name) { return window.API.http('POST', window.API.routes.update(), { id, name }); },
  deleteItem(id) { return window.API.http('DELETE', window.API.routes.delete(id)); },
};


