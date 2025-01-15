<template>
  <div id="app">
    <h1>Voyantis Message Queues</h1>
    <div class="input-groups">
      <div class="input-group">
        <h3>POST Message</h3>
        <input v-model="postMessage.queueName" placeholder="Queue Name" class="input-field" />
        <input v-model="postMessage.message" placeholder="Message" class="input-field" />
        <button @click="postMessageToQueue" class="button">Post Message</button>
        <div v-if="postResponseMessage" class="message">{{ postResponseMessage }}</div>
      </div>

      <div class="input-group">
        <h3>GET Message (Default Timeout)</h3>
        <input v-model="getMessageDefault.queueName" placeholder="Queue Name" class="input-field" />
        <button @click="getMessageWithDefaultTimeout" class="button">Get Message</button>
        <div v-if="getMessageDefault.message && isPressedGetMessage" class="message">{{ getMessageDefault.message }}</div>
      </div>

      <div class="input-group">
        <h3>GET Message (Specific Timeout)</h3>
        <input v-model="getMessageSpecific.queueName" placeholder="Queue Name" class="input-field" />
        <input v-model="getMessageSpecific.timeout" type="number" placeholder="Timeout (ms)" class="input-field" />
        <button @click="getMessageWithTimeout" class="button">Get Message</button>
        <div v-if="getMessageSpecific.message && isPressedGetMessage" class="message">{{ getMessageSpecific.message }}</div>
      </div>

      <div class="input-group queues-list">
        <h3>GET Queues List</h3>
        <button @click="fetchQueues" class="button">Get Queues</button>
        <div v-if="queues.length > 0" class="queue-list">
          <div v-for="queue in queues" :key="queue.name" class="queue-item">
            {{ queue.name }} ({{ queue.count }} messages)
            <div v-if="message[queue.name]  && isPressedGetList" class="message">{{ message[queue.name] }}</div>
          </div>
        </div>
        <div v-else class="message">No queues available.</div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      postMessage: { queueName: '', message: '' },
      getMessageDefault: { queueName: '', message: null },
      getMessageSpecific: { queueName: '', timeout: null, message: null },
      queues: [],
      message: {},
      loading: {},
      selectedQueue: null,
      postResponseMessage: null,
      isPressedGetMessage: false,
      isPressedGetList: false,
    };
  },
  methods: {
    async postMessageToQueue() {
      this.isPressedGetMessage = false;
      this.isPressedGetList = false;
      this.postResponseMessage = null;
      try {
        const response = await axios.post(
          `http://localhost:5170/api/queues/${this.postMessage.queueName}`,
          JSON.stringify(this.postMessage.message),
          {
            headers: {
              'Content-Type': 'application/json',
            },
          }
        );
        this.postResponseMessage = "Message posted successfully";
        this.postMessage.message = "";
        this.fetchQueues();
      } catch (error) {
        console.error("Error posting message:", error);
        this.postResponseMessage = "Error posting message. Check console.";
      }

    },
    async getMessageWithDefaultTimeout() {
      this.getMessageDefault.message = null;
      try {
        const response = await axios.get(`http://localhost:5170/api/queues/${this.getMessageDefault.queueName}`);
        this.getMessageDefault.message = response.data;
      } catch (error) {
        if (error.response && error.response.status === 204) {
          this.getMessageDefault.message = "No message in queue after timeout";
        } else {
          this.getMessageDefault.message = "Error retrieving message, check console";
          console.error("Error getting message (default timeout):", error);
        }
      }
      this.isPressedGetMessage = true;
    },
    async getMessageWithTimeout() {
      this.getMessageSpecific.message = null;
      try {
        const response = await axios.get(`http://localhost:5170/api/queues/${this.getMessageSpecific.queueName}?timeout=${this.getMessageSpecific.timeout}`);
        this.getMessageSpecific.message = response.data;
      } catch (error) {
        if (error.response && error.response.status === 204) {
          this.getMessageSpecific.message = "No message in queue after timeout";
        } else {
          this.getMessageSpecific.message = "Error retrieving message, check console";
          console.error("Error getting message (specific timeout):", error);
        }
      }
    },
    async fetchQueues() {
      try {
        const response = await axios.get('http://localhost:5170/api/queues/list');
        this.queues = response.data;
      } catch (error) {
        console.error("Error fetching queues", error);
      }
      this.isPressedGetList = true;
    },
    async getMessage(queueName) {
      this.loading[queueName] = true;
      try {
        const response = await axios.get(`http://localhost:5170/api/queues/${queueName}`);
        this.message[queueName] = response.data;
      } catch (error) {
        if (error.response && error.response.status === 204) {
          this.message[queueName] = "No message in queue after timeout";
        } else {
          this.message[queueName] = "Error getting message, check console.";
          console.error(`Error getting message from ${queueName}`, error);
        }
      } finally {
        this.loading[queueName] = false;
      }
    },
  },
};
</script>

<style scoped>
#app {
  font-family: 'Arial', sans-serif;
  color: #333;
  padding: 20px;
  height: 93vh;
  background: linear-gradient(180deg, #f2f6ff 0%, #ffffff 100%); /* Similar to Voyantis gradient */
}

h1 {
  display: flex;
  justify-content: center;
  padding-block: 10vh;
  color: #003246;
  font-family: DM Sans, sans-serif;
  font-weight: 400;
  line-height: 1.25;
}
.input-groups {
  display: flex;
  justify-content: center;
  gap: 6vw;
}
.input-group {
  display: flex;
  justify-content: space-between;
  flex-direction: column;
  margin-bottom: 20px;
  border: 1px solid #ddd;
  padding: 40px;
  gap: 10px;
  border-radius: 5px;
}

h3 {
  color: #003246;
  margin-bottom: .625rem;
  font-family: DM Sans, sans-serif;
  font-size: 1.25rem;
  font-weight: 400;
  line-height: 1.625;
  align-self: center;
}

.input-field {
  padding: 8px;
  margin-right: 10px;
  border: 1px solid #ccc;
  border-radius: 3px;
}

.button {
  z-index: 991;
  color: #fff;
  text-align: center;
  align-self: center;
  cursor: pointer;
  background-color: #003246;
  border: 1px solid #f050;
  border-radius: 5px;
  padding: .75rem 1.53125rem;
  min-width: 10vw;
}

.button:hover {
  color: #003246;
  background-color: #0000;
  border: 1px solid #003246;
}

.message {
  text-align: center;
  color: #003246;
}

.queues-list {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.queue-item {
  border: 1px solid #ddd;
  padding: 10px;
  margin-bottom: 5px;
  border-radius: 3px;
}
</style>