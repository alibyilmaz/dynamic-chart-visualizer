/**
 * ConnectionForm component for entering database connection info.
 */
import React, { useState } from 'react';
import { Box, TextField, Button, Typography, Grid } from '@mui/material';

export interface ConnectionInfo {
  host: string;
  database: string;
  username: string;
  password: string;
}

export interface ConnectionFormProps {
  onConnect: (info: ConnectionInfo) => void;
}

const ConnectionForm: React.FC<ConnectionFormProps> = ({ onConnect }) => {
  const [form, setForm] = useState<ConnectionInfo>({
    host: '',
    database: '',
    username: '',
    password: '',
  });
  const [error, setError] = useState('');

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!form.host || !form.database || !form.username || !form.password) {
      setError('All fields are required.');
      return;
    }
    setError('');
    onConnect(form);
  };

  return (
    <Box component="form" onSubmit={handleSubmit} mb={3}>
      <Typography variant="h6" gutterBottom>
        Database Connection
      </Typography>
      <Grid container spacing={2}>
        <Grid item xs={12} sm={6}>
          <TextField label="Host" name="host" value={form.host} onChange={handleChange} fullWidth required />
        </Grid>
        <Grid item xs={12} sm={6}>
          <TextField label="Database" name="database" value={form.database} onChange={handleChange} fullWidth required />
        </Grid>
        <Grid item xs={12} sm={6}>
          <TextField label="Username" name="username" value={form.username} onChange={handleChange} fullWidth required />
        </Grid>
        <Grid item xs={12} sm={6}>
          <TextField label="Password" name="password" value={form.password} onChange={handleChange} type="password" fullWidth required />
        </Grid>
      </Grid>
      {error && <Typography color="error" mt={2}>{error}</Typography>}
      <Button type="submit" variant="contained" sx={{ mt: 2 }}>
        Connect
      </Button>
    </Box>
  );
};

export default ConnectionForm; 