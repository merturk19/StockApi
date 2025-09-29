const { useEffect, useState } = React;

function StockPage() {
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const [newName, setNewName] = useState('');
  const [editId, setEditId] = useState('');
  const [editName, setEditName] = useState('');

  const load = async () => {
    setLoading(true);
    setError('');
    try {
      const data = await window.API.getAllItems();
      setItems(Array.isArray(data) ? data : []);
    } catch (e) {
      setError(e.message || 'Failed to load');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { load(); }, []);

  const canCreate = newName.trim().length > 0;
  const canUpdate = String(editId).trim().length > 0 && editName.trim().length > 0;

  const onCreate = async () => {
    if (!canCreate) return;
    await window.API.addItem(newName.trim());
    setNewName('');
    await load();
  };

  const onUpdate = async () => {
    if (!canUpdate) return;
    await window.API.updateItem(Number(editId), editName.trim());
    setEditId('');
    setEditName('');
    await load();
  };

  const onDelete = async (id) => {
    await window.API.deleteItem(id);
    await load();
  };

  return (
    <div className="container">
      <div className="card">
        <h1>Stock Items</h1>
        <div className="toolbar">
          <span className="pill">API base: /stockapi/StockItem</span>
          {loading ? <span className="status">Loading…</span> : null}
          {error ? <span className="status">Error: {error}</span> : null}
        </div>

        <div className="row">
          <div className="field">
            <label>New item name</label>
            <input type="text" value={newName} onChange={(e) => setNewName(e.target.value)} placeholder="e.g. Keyboard" />
          </div>
          <div className="actions">
            <button className="primary" disabled={!canCreate || loading} onClick={onCreate}>Add</button>
          </div>
        </div>

        <div className="row" style={{ marginTop: 10 }}>
          <div className="field">
            <label>Edit item id</label>
            <input type="number" value={editId} onChange={(e) => setEditId(e.target.value)} placeholder="e.g. 3" />
          </div>
          <div className="field">
            <label>New name</label>
            <input type="text" value={editName} onChange={(e) => setEditName(e.target.value)} placeholder="Updated name" />
          </div>
          <div className="actions">
            <button disabled={!canUpdate || loading} onClick={onUpdate}>Update</button>
          </div>
        </div>

        <div className="list">
          {items.length === 0 ? (
            <div className="empty">No items yet. Add one above.</div>
          ) : items.map((it) => (
            <div key={it.id} className="item">
              <div className="pill">#{it.id}</div>
              <div>{it.name}</div>
              <div style={{ display: 'flex', gap: 8, justifyContent: 'flex-end' }}>
                <button className="ghost" onClick={() => { setEditId(String(it.id)); setEditName(it.name || ''); }}>Edit</button>
                <button className="danger" onClick={() => { if (confirm(`Delete item #${it.id}?`)) onDelete(it.id); }}>Delete</button>
              </div>
            </div>
          ))}
        </div>

        <div className="footer">
          <span>Total: {items.length}</span>
          <a href="/swagger" target="_blank" rel="noreferrer">Open API Docs</a>
        </div>
      </div>
    </div>
  );
}

window.StockPage = StockPage;


