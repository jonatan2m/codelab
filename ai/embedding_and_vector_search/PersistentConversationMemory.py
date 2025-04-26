import json
from langchain_core.memory import BaseMemory
from langchain_core.messages import HumanMessage, AIMessage, messages_from_dict, messages_to_dict

class PersistentConversationMemory(BaseMemory):
    @property
    def memory_variables(self):
        return ["chat_history"]

    def __init__(self, storage_path="history.json"):
        super().__init__()
        self._storage_path = storage_path

    def load_history(self):
        try:
            with open(self._storage_path, "r") as f:
                messages = json.load(f)
                return messages_from_dict(messages)
        except FileNotFoundError:
            return []

    def save_history(self, history):
        with open(self._storage_path, "w") as f:
            json.dump(messages_to_dict(history), f, indent=2)

    def load_memory_variables(self, inputs):
        history = self.load_history()
        return {"chat_history": history}

    def save_context(self, inputs, outputs):
        history = self.load_history()
        history.append(HumanMessage(content=inputs["question"]))
        history.append(AIMessage(content=outputs["answer"]))
        self.save_history(history)

    def clear(self):
        self.save_history([])
