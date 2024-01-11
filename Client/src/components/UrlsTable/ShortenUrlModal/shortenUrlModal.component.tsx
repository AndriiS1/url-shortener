import {
  Box,
  Button,
  Fade,
  FormControl,
  FormLabel,
  Modal,
  Snackbar,
  TextField,
  TextareaAutosize,
} from "@mui/material";
import "./shortenUrlModal.component.style.css";
import { Form } from "react-router-dom";

export default function ShortenUrlModal(props: {
  open: boolean;
  setOpen: (a: boolean) => void;
}) {
  return (
    <>
      <Modal
        open={props.open}
        onClose={() => props.setOpen(false)}
        closeAfterTransition
      >
        <Fade in={props.open}>
          <Box className="modal-box">
            <Form className="add-modal-form" onSubmit={() => {}}>
              <span className="form-title">Shorten url</span>
              <FormControl required className="add-modal-element">
                <FormLabel className="label">Input your url</FormLabel>
                <TextField multiline rows={4} maxRows={20} />
              </FormControl>
              <div className="add-modal-button-container">
                <Button type="submit">Submit</Button>
              </div>
            </Form>
          </Box>
        </Fade>
      </Modal>
      <Snackbar
        // open={openError}
        // onClose={handleCloseError}
        autoHideDuration={4000}
        message="Active task limit is 5."
      />
    </>
  );
}
