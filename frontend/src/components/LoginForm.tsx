/**
 * LoginForm component for user authentication.
 */
import React, { useState } from 'react';
import { Box, TextField, Button, Typography, Paper } from '@mui/material';

export interface LoginFormProps {
  onLogin: (username: string, password: string) => void;
  error?: string;
}

const LoginForm: React.FC<LoginFormProps> = ({ onLogin, error }) => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [localError, setLocalError] = useState('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!username || !password) {
      setLocalError('Username and password are required.');
      return;
    }
    setLocalError('');
    onLogin(username, password);
  };

  return (
    <Paper elevation={3} sx={{ p: 3, maxWidth: 400, mx: 'auto', mt: 6 }}>
      <Box component="form" onSubmit={handleSubmit}>
        <Typography variant="h6" gutterBottom>
          Login
        </Typography>
        <TextField
          label="Username"
          value={username}
          onChange={e => setUsername(e.target.value)}
          fullWidth
          margin="normal"
          required
        />
        <TextField
          label="Password"
          type="password"
          value={password}
          onChange={e => setPassword(e.target.value)}
          fullWidth
          margin="normal"
          required
        />
        {(localError || error) && (
          <Typography color="error" mt={2}>
            {localError || error}
          </Typography>
        )}
        <Button type="submit" variant="contained" sx={{ mt: 2 }} fullWidth>
          Login
        </Button>
      </Box>
    </Paper>
  );
};

export default LoginForm; 