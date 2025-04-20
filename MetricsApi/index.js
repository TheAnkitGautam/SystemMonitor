const express = require('express');
const bodyParser = require('body-parser');
const app = express();
const port = 5000;

// Middleware to parse JSON
app.use(bodyParser.json());

// POST endpoint to receive metrics
app.post('/api/metrics', (req, res) => {
  const { cpu, ram_used, ram_total, disk_used, disk_total } = req.body;
  
  console.log(`[${new Date().toISOString()}] Received metrics:`, {
    cpu: `${cpu}`,
    ram_used: `${ram_used}`,
    ram_total: `${ram_total}`,
    disk: `${disk_used}`,
    disk_total: `${disk_total}`,
  });
  res.status(200).json({ status: 'success' });
});

// Start server
app.listen(port, () => {
  console.log(`Server running at http://localhost:${port}`);
});