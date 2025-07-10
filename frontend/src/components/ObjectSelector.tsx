/**
 * ObjectSelector component for selecting data object type and name.
 */
import React from 'react';
import { Box, FormControl, InputLabel, Select, MenuItem, Typography, Grid } from '@mui/material';
import { DataObjectType } from '../App';

export interface ObjectSelectorProps {
  objects: string[];
  type: DataObjectType;
  selectedObject: string | null;
  onTypeChange: (type: DataObjectType) => void;
  onSelect: (objectName: string) => void;
}

const ObjectSelector: React.FC<ObjectSelectorProps> = ({ objects, type, selectedObject, onTypeChange, onSelect }) => (
  <Box mb={3}>
    <Typography variant="h6" gutterBottom>
      Select Data Object
    </Typography>
    <Grid container spacing={2}>
      <Grid item xs={12} sm={6}>
        <FormControl fullWidth>
          <InputLabel>Type</InputLabel>
          <Select
            value={type}
            label="Type"
            onChange={e => onTypeChange(e.target.value as DataObjectType)}
          >
            <MenuItem value={DataObjectType.View}>View</MenuItem>
            <MenuItem value={DataObjectType.Procedure}>Stored Procedure</MenuItem>
            <MenuItem value={DataObjectType.Function}>Function</MenuItem>
          </Select>
        </FormControl>
      </Grid>
      <Grid item xs={12} sm={6}>
        <FormControl fullWidth>
          <InputLabel>Object</InputLabel>
          <Select
            value={selectedObject || ''}
            label="Object"
            onChange={e => onSelect(e.target.value)}
          >
            {objects.map(obj => (
              <MenuItem key={obj} value={obj}>{obj}</MenuItem>
            ))}
          </Select>
        </FormControl>
      </Grid>
    </Grid>
  </Box>
);

export default ObjectSelector; 