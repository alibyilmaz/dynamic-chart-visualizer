/**
 * DataMapper component for mapping data columns to chart axes and labels.
 */
import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import FormControl from '@mui/material/FormControl';
import InputLabel from '@mui/material/InputLabel';
import Select from '@mui/material/Select';
import MenuItem from '@mui/material/MenuItem';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';

export interface Mapping {
  x: string;
  y: string;
  label?: string;
}

export interface DataMapperProps {
  columns: string[];
  onMappingChange: (mapping: Mapping) => void;
}

const DataMapper: React.FC<DataMapperProps> = ({ columns, onMappingChange }) => {
  const [mapping, setMapping] = useState<Mapping>({ x: '', y: '', label: '' });

  useEffect(() => {
    if (mapping.x && mapping.y) {
      onMappingChange(mapping);
    }
    // eslint-disable-next-line
  }, [mapping]);

  // Defensive: If mapping.label is not in columns, set to ''
  const labelValue = mapping.label && columns.includes(mapping.label) ? mapping.label : '';

  return (
    <Box mb={3}>
      <Typography variant="h6" gutterBottom>
        Map Data Fields
      </Typography>
      <Grid container spacing={2} alignItems="center">
        <Grid item xs={12} sm={4}>
          <FormControl fullWidth>
            <InputLabel>X Axis</InputLabel>
            <Select
              value={mapping.x}
              label="X Axis"
              onChange={e => setMapping({ ...mapping, x: e.target.value })}
            >
              {columns.map(col => (
                <MenuItem key={col} value={col}>{col}</MenuItem>
              ))}
            </Select>
          </FormControl>
        </Grid>
        <Grid item xs={12} sm={4}>
          <FormControl fullWidth>
            <InputLabel>Y Axis</InputLabel>
            <Select
              value={mapping.y}
              label="Y Axis"
              onChange={e => setMapping({ ...mapping, y: e.target.value })}
            >
              {columns.map(col => (
                <MenuItem key={col} value={col}>{col}</MenuItem>
              ))}
            </Select>
          </FormControl>
        </Grid>
        <Grid item xs={12} sm={4}>
          <FormControl fullWidth>
            <InputLabel>Labels (optional)</InputLabel>
            <Select
              value={labelValue}
              label="Labels (optional)"
              onChange={e => setMapping({ ...mapping, label: e.target.value })}
            >
              <MenuItem value="">None</MenuItem>
              {columns.map(col => (
                <MenuItem key={col} value={col}>{col}</MenuItem>
              ))}
            </Select>
          </FormControl>
        </Grid>
      </Grid>
    </Box>
  );
};

export default DataMapper; 